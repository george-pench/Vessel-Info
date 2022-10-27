namespace Vessel_Info.Data.Constants
{
    public class DataConstants
    {
        // TODO: add variables for recurring numbers

        public class User
        {
            public const int NameMinLength = 4;
            public const int NameMaxLength = 50;
            public const int PasswordMinLength = 8;
            public const int PasswordMaxLength = 50;
        }

        public class Vessel
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 100;
            public const int ImoMinLength = 7;
            public const int ImoMaxLength = 7;
            public const int LoaMinLength = 5;
            public const int LoaMaxLength = 500;
            public const int CubicMinLength = 1000;
            public const int CubicMaxLength = 500000;
            public const int HullTypeMinLength = 2;
            public const int HyllTypeMaxLength = 40;            
            public const int CallSignMinLength = 5;
            public const int CallSignMaxLength = 40;
            public const int SummertDwtMinLength = 1000;
            public const int SummertDwtMaxLength = 600000;
            public const int BuiltMinValue = 1900;
            public const int BuiltMaxValue = 2050;
            public const int DraftMinValue = 5;
            public const int DraftMaxValue = 40;
            public const int BeamMinValue = 8;
            public const int BeamMaxValue = 100;
        }

        public class Type
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 200;
        }

        public class Shipbroker
        {
            public const int AgencyNameMinLength = 4;
            public const int AgencyNameMaxLength = 200;
            public const int TelephoneNumberMinLength = 8;
            public const int TelephoneNumberMaxLength = 20;
        }

        public class Registration
        {
            public const int FlagMinLength = 3;
            public const int FlagMaxLength = 50;
            public const int RegistryPortMinLength = 3;
            public const int RegistryPortMaxLength = 50;
        }

        public class Owner
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 200;
            public const int FoundedMinValue = 1890;
            public const int FoundedMaxValue = 2040;
            public const int WebsiteMinLength = 10;
            public const int WebsiteMaxLength = 50;
        }

        public class Operator
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 200;
            public const int FoundedMinValue = 1890;
            public const int FoundedMaxValue = 2040;
            public const int WebsiteMinLength = 10;
            public const int WebsiteMaxLength = 50;
        }

        public class ClassificationSociety
        {
            public const int FullNameMinLength = 10;
            public const int FullNameMaxLength = 200;
            public const int AbbreviationMinLength = 2;
            public const int AbbreviationMaxLength = 10;
            public const int FoundedMinValue = 1760;
            public const int FoundedMaxValue = 2040;
            public const int WebsiteMinLength = 8;
            public const int WebsiteMaxLength = 50;
        }
    }
}
