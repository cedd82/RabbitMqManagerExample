namespace basicExampleWorkingWithRabbitMq;

public class QueueMessageTyped
{
    public string ProcessReference { get; set; }
    public string EventCode { get; set; }
    public string ProcessDate { get; set; }
    public string ProcessUserCode { get; set; }
    public int DriverID { get; set; }
    public string DriverName { get; set; }
    public int FleetId { get; set; }
    public string MovementCode { get; set; }
    public int ManifestId { get; set; }
    public string LocationId { get; set; }
    public string LocationCode { get; set; }
    public string LocationDropSequence { get; set; }
    public string AppType { get; set; }
    public string AppVersion { get; set; }
    public string DeviceName { get; set; }
    public string DeviceID { get; set; }
    public string CategoryCode { get; set; }
    public string DepotCode { get; set; }
    public VehicleProcessData VehicleProcessData { get; set; }
    public CreateFactsVehicleData CreateFactsVehicleData { get; set; }
    public YardProcessData YardProcessData { get; set; }
    public OutboundPickProcessData OutboundPickProcessData { get; set; }
    public List<ManifestVehicle> Vehicles { get; set; }
}

public class YardProcessData
{
    public string ProcessCode { get; set; }
    public string SiteCode { get; set; }
    public string AccountCode { get; set; }
    public string Vin { get; set; }
    public string ColorName { get; set; }
    public string ToStockCode { get; set; }
    public string Notes { get; set; }
    public string DamageCode { get; set; }
    public string DamageCodeJson { get; set; }
    public int ToLocationId { get; set; }
}

public class CreateFactsVehicleData
{
    public string Vin { get; set; }
    public string SiteCode { get; set; }

    public string AccountCode { get; set; }
    public string VehicleId { get; set; }
    public string MakeName { get; set; }
    public string ModelName { get; set; }
    public string StockCode { get; set; }

    public string CompanyCode { get; set; }

    public string ColorName { get; set; }

    public string CustomerStockNumber { get; set; }

    public string VehicleTypeName { get; set; }

    public int FactsBookingNumber { get; set; }

    public int FactsBookingVehicleNumber { get; set; }
    public string SurveyType { get; set; }

    public string VehicleInspectionSurveyCode { get; set; }
    public string VehicleBillingRateGroupCode { get; set; }

    public string VehicleBillingRateGroupDescription { get; set; }
    public int ToLocationId { get; set; }
    public string SurveyConditionName { get; set; }
    public string SurveyDamageName { get; set; }
    public string SurveySilhouetteName { get; set; }
    public bool? IsSilhouette { get; set; }
    public string DepotCode { get; set; }
    public string VehicleTypeCode { get; set; }
    public string DamageCodeJson { get; set; }
}

public class OutboundPickProcessData
{
    public int VehicleOutboundId { get; set; }
    public string Vin { get; set; }
    public string SiteCode { get; set; }
    public string AccountCode { get; set; }
    public string ToStockCode { get; set; }
    public string DamageCode { get; set; }
    public string Notes { get; set; }
    public string DamageCodeJson { get; set; }
    public int? ToOdometer { get; set; }
}

public class VehicleProcessData
{
    public VehicleDetail Detail { get; set; }
    public Conditions Conditions { get; set; }
    public Damages Damages { get; set; }
    public Photos Photos { get; set; }
    public Signature Signature { get; set; }
}

public class Conditions
{
    public string VehicleColor { get; set; }
    public string SurveyConditionsName { get; set; }
    public bool IsOe { get; set; }
    public bool IsBike { get; set; }
    public string AdditionalComment { get; set; }
    public string SurveyConditionsString { get; set; }
    public string SurveyConditionsResponse { get; set; }
}

public class SurveyConditionsResponse
{
    public string SurveyCondition { get; set; }
    public string Windscreen { get; set; }
    public string Battery { get; set; }
    public bool SpareTyre { get; set; }
    public string StoneChips { get; set; }
    public bool Drivable { get; set; }
    public int Keys { get; set; }
    public string Fuel { get; set; }
    public string InteriorCondition { get; set; }
    public bool StereoFitted { get; set; }
    public string TravelHeight { get; set; }
    public long Odometer { get; set; }
}

public class Damages
{
    public string SurveyDamagesName { get; set; }
    public string SurveyDamageCodesResponse { get; set; }
    public bool IsSilhouette { get; set; }
    public string SilhouetteName { get; set; }
    public string SilhouetteResponseImage { get; set; }
    public string SilhouetteResponseCoordinates { get; set; }
    public bool IsDamaged { get; set; }
}

public class VehicleDetail
{
    public string PickupDeliveryStatusCode { get; set; }
    public int? BookingNumber { get; set; }
    public short? BookingVehicleNumber { get; set; }
    public string CevaBarcode { get; set; }
    public string VehicleId { get; set; }
    public string OtherId { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public string VehicleType { get; set; }
    public int VehicleInspectionSurveyCode { get; set; }
    public string VehicleBillingRateGroupCode { get; set; }
    public string VehicleBillingRateGroupDescription { get; set; }
    public bool IsScannable { get; set; }
    public bool IsAgentPickup { get; set; }
    public bool IsSurveyRequired { get; set; }
    public string PreviousProcessReference { get; set; }
}

public class Photos
{
    public long MaximumPhotosRequired { get; set; }
    public long MinimumPhotosRequired { get; set; }
    public long MinimumPhotosRequiredIsDamaged { get; set; }
    public long MaximumPhotosRequiredIsDamaged { get; set; }
    public List<PhotosList> PhotosList { get; set; }
}

public class PhotosList
{
    public string PhotoName { get; set; }
    public bool IsProfile { get; set; }
    public string PhotoBody { get; set; }
    public string PhotoType { get; set; }
}

public class Signature
{
    public bool IsRequired { get; set; }
    public bool IsGroupSignature { get; set; }
    public string Name { get; set; }
    public string Comment { get; set; }
    public string AdditionalComment { get; set; }
    public string SignatureDataImage { get; set; }
    public string SignatureDataConditions { get; set; }
    public string SignatureDataCoordinates { get; set; }
    public List<TermsAndCondition> TermsAndConditions { get; set; }
}

public class TermsAndCondition
{
    public bool IsAccepted { get; set; }
    public string Terms { get; set; }
}

public class ManifestVehicle
{
    public int BookingNumber { get; set; }
    public int BookingVehicleNumber { get; set; }
    public int ManifestId { get; set; }
}