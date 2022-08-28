namespace Vessel_Info.Services.Constants
{
    public class ServicesConstants
    {
        public const char START_LETTER = 'X';
        public const char END_LETTER = 'Y';

        public const string BASE_URL = "https://www.q88.com/ships.aspx?letter={0}&v=list";
        public const string VIEW_SHIP_URL = "https://www.q88.com/ViewShip.aspx?id={0}";
        public const string ERROR_MESSAGE = "No vessels starting with";

        public const string VESSEL_NAME_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td > a";
        public const string IMO_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(2)";
        public const string BUILT_DATA_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(3)";
        public const string DWT_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(4)";
        public const string LOA_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(5)";
        public const string CUBIC_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(6)";
        public const string BEAM_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(7)";
        public const string DRAFT_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(8)";
        public const string HULL_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(9)";
        public const string CALL_SIGN_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(10)";
        public const string OWNER_SELECTOR = "#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(6) > td:nth-child(5)";
        public const string TYPE_SELECTOR = "#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(5) > td:nth-child(2)";
        public const string GUID_SELECTOR = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(1)";
        public const string FLAG_SELECTOR = "#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(3) > td:nth-child(2)";
        public const string PORT_SELECTOR = "#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(4) > td:nth-child(2)";
        public const string CLASS_SOCIETY_SELECTOR = "#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(7) > td:nth-child(2)";

        public const int SKIPNUMBER_SELECTOR = 1;
    }
}
