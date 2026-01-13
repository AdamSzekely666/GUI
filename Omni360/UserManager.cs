using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace MatroxLDS
{
    /// <summary>
    /// Manages card->username mapping stored inside config.json (next to the exe).
    /// It reads/writes the "Cards" object in the config file.
    /// </summary>
    public class UsersManager
    {
        private readonly string _configPath;
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private Dictionary<string, string> _cardToUser = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // In-memory users reference (MainForm.User objects)
        private IList<MainForm.User> _users;

        public UsersManager(IList<MainForm.User> users)
        {
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            Load();
        }

        /// <summary>
        /// Replace the in-memory users reference used for username validation.
        /// </summary>
        public void SetUsers(IList<MainForm.User> users)
        {
            if (users == null) throw new ArgumentNullException(nameof(users));
            _users = users;
        }

        /// <summary>
        /// Load cards mapping from config.json (if present).
        /// </summary>
        public void Load()
        {
            _lock.EnterWriteLock();
            try
            {
                _cardToUser = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                if (!File.Exists(_configPath))
                    return;

                var json = File.ReadAllText(_configPath);
                if (string.IsNullOrWhiteSpace(json))
                    return;

                try
                {
                    var j = JObject.Parse(json);
                    var cardsToken = j["Cards"];
                    if (cardsToken != null && cardsToken.Type == JTokenType.Object)
                    {
                        var dict = cardsToken.ToObject<Dictionary<string, string>>();
                        if (dict != null)
                            _cardToUser = new Dictionary<string, string>(dict, StringComparer.OrdinalIgnoreCase);
                    }
                }
                catch (JsonException)
                {
                    // invalid config.json format — ignore cards
                }
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Save the in-memory cards mapping into config.json's "Cards" property.
        /// Performs atomic write: tmp -> replace.
        /// Safe to call whether or not caller already holds write lock.
        /// </summary>
        public void Save()
        {
            // If already holding write lock just write directly
            if (_lock.IsWriteLockHeld)
            {
                SaveToConfigJson();
                return;
            }

            _lock.EnterWriteLock();
            try
            {
                SaveToConfigJson();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        private void SaveToConfigJson()
        {
            // Read existing config (if any), set/replace Cards property, write atomically
            JObject root = new JObject();
            if (File.Exists(_configPath))
            {
                try
                {
                    var existing = File.ReadAllText(_configPath);
                    if (!string.IsNullOrWhiteSpace(existing))
                    {
                        root = JObject.Parse(existing);
                    }
                }
                catch
                {
                    // If parse fails, overwrite with a fresh object (but don't throw)
                    root = new JObject();
                }
            }

            // Build a JObject for cards
            var cardsObj = JObject.FromObject(_cardToUser ?? new Dictionary<string, string>());
            root["Cards"] = cardsObj;

            // Atomic write
            var tmp = _configPath + ".tmp";
            File.WriteAllText(tmp, root.ToString(Formatting.Indented));
            File.Copy(tmp, _configPath, true);
            File.Delete(tmp);
        }

        /// <summary>Return username for cardId, or null.</summary>
        public string GetUserByCardId(string cardId)
        {
            if (string.IsNullOrWhiteSpace(cardId)) return null;
            _lock.EnterReadLock();
            try
            {
                _cardToUser.TryGetValue(cardId.Trim(), out var user);
                return user;
            }
            finally { _lock.ExitReadLock(); }
        }

        /// <summary>Enrolls cardId to username (overwrites previous mapping if any). Returns previous owner or null.</summary>
        public string EnrollCardToUser(string username, string cardId)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(cardId))
                throw new ArgumentException("username/cardId required");
            username = username.Trim();
            cardId = cardId.Trim();

            // Ensure username exists in config users
            if (!_users.Any(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Unknown username");

            _lock.EnterWriteLock();
            try
            {
                string previous = null;
                if (_cardToUser.TryGetValue(cardId, out previous) && previous == username)
                {
                    // already assigned to same user
                    return previous;
                }

                _cardToUser[cardId] = username;
                Save(); // writes into config.json
                return previous;
            }
            finally { _lock.ExitWriteLock(); }
        }

        public void RemoveCardFromUser(string username, string cardId)
        {
            _lock.EnterWriteLock();
            try
            {
                if (_cardToUser.TryGetValue(cardId, out var existing) && string.Equals(existing, username, StringComparison.OrdinalIgnoreCase))
                {
                    _cardToUser.Remove(cardId);
                    Save();
                }
            }
            finally { _lock.ExitWriteLock(); }
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllMappings()
        {
            _lock.EnterReadLock();
            try { return _cardToUser.ToList(); }
            finally { _lock.ExitReadLock(); }
        }
    }
}