namespace Vessel_Info.Services.Constants
{
    public class ServicesConstants
    {
        public const char StartLetter = 'X';
        public const char EndLetter = 'Y';

        public const string BaseUrl = "https://www.q88.com/ships.aspx?letter={0}&v=list";
        public const string ViewShipUrl = "https://www.q88.com/ViewShip.aspx?id={0}";
        public const string ErrorMessage = "No vessels starting with";

        public const string VesselNameSelector = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td > a";
        public const string ImoSelector = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(2)";
        public const string BuiltDataSelector = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(3)";
        public const string DwtSelector = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(4)";
        public const string LoaSelector = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(5)";
        public const string CubicSelector = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(6)";
        public const string BeamSelector = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(7)";
        public const string DraftSelector = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(8)";
        public const string HullSelector = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(9)";
        public const string CallSignSelector = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(10)";
        public const string OwnerSelector = "#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(6) > td:nth-child(5)";
        public const string TypeSelector = "#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(5) > td:nth-child(2)";
        public const string GuidSelector = "#ctl00_cphMiddle_ctl00_modView_dgVessel > tbody > tr > td:nth-child(1)";
        public const string FlagSelector = "#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(3) > td:nth-child(2)";
        public const string PortSelector = "#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(4) > td:nth-child(2)";
        public const string ClassSocietySelector = "#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(7) > td:nth-child(2)";
        public const string OperatorSelector = "#pnlQuestionnaires > table.main > tbody > tr:nth-child(1) > td > table > tbody > tr:nth-child(7) > td:nth-child(5)";

        public const int ScrapeVesselSkipNumber = 1;
        public const int ScrapeSkipNumber = 0;
    }
}
