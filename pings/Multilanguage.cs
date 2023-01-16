using System.Globalization;

namespace pings
{
    public class Multilanguage
    {
        private static string _lang;

        static Multilanguage()
        {
            try
            {
                CultureInfo _culture = CultureInfo.InstalledUICulture;
                _lang = _culture.TwoLetterISOLanguageName;
            }
            catch (System.Exception)
            {
                _lang = "en";
            }
        }

        public static string Name
        {
            get
            {
                switch (_lang)
                {
                    case "ru":
                        {
                            return Resource.ru_name;
                        }
                    case "en":
                        {
                            return Resource.en_name;
                        }
                    default:
                        {
                            return Resource.en_name;
                        }
                }
            }
        }

        public static string IP
        {
            get
            {
                switch (_lang)
                {
                    case "ru":
                        {
                            return Resource.ru_ip;
                        }
                    case "en":
                        {
                            return Resource.en_ip;
                        }
                    default:
                        {
                            return Resource.en_ip;
                        }
                }
            }
        }

        public static string Time
        {
            get
            {
                switch (_lang)
                {
                    case "ru":
                        {
                            return Resource.ru_time;
                        }
                    case "en":
                        {
                            return Resource.en_time;
                        }
                    default:
                        {
                            return Resource.en_time;
                        }
                }
            }
        }

        public static string Loss
        {
            get
            {
                switch (_lang)
                {
                    case "ru":
                        {
                            return Resource.ru_loss;
                        }
                    case "en":
                        {
                            return Resource.en_loss;
                        }
                    default:
                        {
                            return Resource.en_loss;
                        }
                }
            }
        }

        public static string History
        {
            get
            {
                switch (_lang)
                {
                    case "ru":
                        {
                            return Resource.ru_history;
                        }
                    case "en":
                        {
                            return Resource.en_history;
                        }
                    default:
                        {
                            return Resource.en_history;
                        }
                }
            }
        }

        public static string Ms
        {
            get
            {
                switch (_lang)
                {
                    case "ru":
                        {
                            return Resource.ru_ms;
                        }
                    case "en":
                        {
                            return Resource.en_ms;
                        }
                    default:
                        {
                            return Resource.en_ms;
                        }
                }
            }
        }

        public static string ErrFileNotFound
        {
            get
            {
                switch (_lang)
                {
                    case "ru":
                        {
                            return Resource.ru_err_file_not_found;
                        }
                    case "en":
                        {
                            return Resource.en_err_file_not_found;
                        }
                    default:
                        {
                            return Resource.en_err_file_not_found;
                        }
                }
            }
        }
    }
}



