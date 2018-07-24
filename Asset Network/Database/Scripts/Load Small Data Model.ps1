# Configuration
$projectRoot = $(split-path -parent $(split-path -parent $(split-path -parent $(split-path -parent $SCRIPT:MyInvocation.MyCommand.Path))))
$scriptLoader = "C:\Program Files\Teraque\Asset Network\Script Loader\Script Loader.exe"

# Configuration
&"${scriptLoader}" -f -i "${projectRoot}\Asset Network\Database\Data\Configuration.xml"
	
# Constants
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\CommissionType.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\CommissionUnit.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Condition.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\CreditRatingService.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\CreditRating.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\HolidayType.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\LotHandling.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\MajorInstrumentType.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\MinorInstrumentType.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\MssaBucket.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\OrderType.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\PartyType.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\PositionType.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Property.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\SettlementUnit.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\State.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Status.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Side.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\SubordinateType.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Crossing.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\TimeInForce.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\TimeUnit.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Volume Category.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Weekend.xml"

# Data
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Image.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Type.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\TypeTree.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Country.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Province.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Exchange.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Issuer.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Currency.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\CA Equity.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\UK Equity.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\US Equity.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Debt.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Delinquent Debt Price.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Debt Attribute.xml"
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Corporate Logo.xml"
 
# Sample Destinations
&"${scriptLoader}" -i "${projectRoot}\Asset Network\Database\Data\Destination.xml"
