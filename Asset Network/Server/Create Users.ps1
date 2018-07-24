# Constants
set-variable -name ADS_UF_ACCOUNTDISABLE -value 2 -option constant
set-variable -name ADS_SECURE_AUTHENTICATION -value 1 -option constant
set-variable -name ADS_USE_ENCRYPTION -value 2 -option constant
set-variable -name ADS_USE_SSL -value 2 -option constant
set-variable -name ADS_READONLY_SERVER -value 4 -option constant
set-variable -name ADS_PROMPT_CREDENTIALS -value 8 -option constant
set-variable -name ADS_NO_AUTHENTICATION -value 16 -option constant
set-variable -name ADS_FAST_BIND -value 32 -option constant
set-variable -name ADS_USE_SIGNING -value 64 -option constant
set-variable -name ADS_USE_SEALING -value 128 -option constant
set-variable -name ADS_USE_DELEGATION -value 256 -option constant
set-variable -name ADS_SERVER_BIND -value 512 -option constant
set-variable -name ADS_OPTION_PASSWORD_PORTNUMBER -value 6 -option constant
set-variable -name ADS_OPTION_PASSWORD_METHOD -value 7 -option constant
set-variable -name ADS_PASSWORD_ENCODE_REQUIRE_SSL -value 0 -option constant
set-variable -name ADS_PASSWORD_ENCODE_CLEAR -value 1 -option constant

# This is the LDAP port for managing passwords for the users.
$port = 389

# The root of the domain
$domain = [ADSI]"LDAP://localhost/O=Dark Bond,DC=darkbond,DC=com"

# The roles
$roles = [ADSI]"LDAP://localhost/CN=Roles,O=Dark Bond,DC=darkbond,DC=com"

# Administrators
$roleAdministrators = [ADSI]"LDAP://localhost/CN=Administrators,CN=Roles,O=Dark Bond,DC=darkbond,DC=com"

# TenantAdministrators
write-host "Group: TenantAdministrators"
$roleTenantAdministrators = $roles.Create("group", "CN=TenantAdministrators")
$roleTenantAdministrators.SetInfo()

# Managers
write-host "Group: Managers"
$roleManagers = $roles.Create("group", "CN=Managers")
$roleManagers.SetInfo()

# Traders
write-host "Group: Traders"
$roleTraders = $roles.Create("group", "CN=Traders")
$roleTraders.SetInfo()

# Administrator
write-host "User: Administrator"
$userAdministrator = $domain.Create("user", "CN=Administrator")
$userAdministrator.Put("displayName", "Administrator")
$userAdministrator.Put("givenName", "Administrator")
$userAdministrator.Put("name", "Administrator")
$userAdministrator.SetInfo()
$userAdministrator.AuthenticationType = $ADS_SECURE_AUTHENTICATION -bor $ADS_USE_SIGNING -bor $ADS_USE_SEALING
$userAdministrator.Invoke("SetOption", $ADS_OPTION_PASSWORD_PORTNUMBER, $port)
$userAdministrator.Invoke("SetOption", $ADS_OPTION_PASSWORD_METHOD, $ADS_PASSWORD_ENCODE_CLEAR)
$userAdministrator.Invoke(”setPassword”,"Teraque")
$userAdministrator.InvokeSet("accountDisabled", $false)
$userAdministrator.SetInfo()
$roleAdministrators.member = $roleAdministrators.member + $userAdministrator.distinguishedName
$roleAdministrators.SetInfo()

# Aspen Group
write-host "Organization: Aspen Group"
$organizationalUnitAspenGroup = $domain.Create("organizationalUnit", "OU=Aspen Group")
$organizationalUnitAspenGroup.SetInfo()

# Administrator
write-host "`tUser: Administrator"
$userAdministrator = $organizationalUnitAspenGroup.Create("user", "CN=Administrator")
$userAdministrator.Put("displayName", "Administrator")
$userAdministrator.Put("givenName", "Administrator")
$userAdministrator.Put("name", "Administrator")
$userAdministrator.SetInfo()
$userAdministrator.AuthenticationType = $ADS_SECURE_AUTHENTICATION -bor $ADS_USE_SIGNING -bor $ADS_USE_SEALING
$userAdministrator.Invoke("SetOption", $ADS_OPTION_PASSWORD_PORTNUMBER, $port)
$userAdministrator.Invoke("SetOption", $ADS_OPTION_PASSWORD_METHOD, $ADS_PASSWORD_ENCODE_CLEAR)
$userAdministrator.Invoke(”setPassword”,"Teraque")
$userAdministrator.InvokeSet("accountDisabled", $false)
$userAdministrator.SetInfo()
$roleTenantAdministrators.member = $roleTenantAdministrators.member + $userAdministrator.distinguishedName
$roleTenantAdministrators.SetInfo()

# Alice Wong
write-host "`tUser: Alice Wong"
$userAliceWong = $organizationalUnitAspenGroup.Create("user", "CN=Alice Wong")
$userAliceWong.Put("displayName", "Alice Wong")
$userAliceWong.Put("givenName", "Alice")
$userAliceWong.Put("name", "Alice Wong")
$userAliceWong.Put("sn", "Wong")
$userAliceWong.SetInfo()
$userAliceWong.AuthenticationType = $ADS_SECURE_AUTHENTICATION -bor $ADS_USE_SIGNING -bor $ADS_USE_SEALING
$userAliceWong.Invoke("SetOption", $ADS_OPTION_PASSWORD_PORTNUMBER, $port)
$userAliceWong.Invoke("SetOption", $ADS_OPTION_PASSWORD_METHOD, $ADS_PASSWORD_ENCODE_CLEAR)
$userAliceWong.Invoke(”setPassword”,"Teraque")
$userAliceWong.InvokeSet("accountDisabled", $false)
$userAliceWong.SetInfo()
$roleTraders.member = $roleTraders.member + $userAliceWong.distinguishedName
$roleTraders.SetInfo()

# Dev Kapoor
write-host "`tUser: Dev Kapoor"
$userDevKapoor = $organizationalUnitAspenGroup.Create("user", "CN=Dev Kapoor")
$userDevKapoor.Put("displayName", "Dev Kapoor")
$userDevKapoor.Put("givenName", "Dev")
$userDevKapoor.Put("name", "Dev Kapoor")
$userDevKapoor.Put("sn", "Kapoor")
$userDevKapoor.SetInfo()
$userDevKapoor.AuthenticationType = $ADS_SECURE_AUTHENTICATION -bor $ADS_USE_SIGNING -bor $ADS_USE_SEALING
$userDevKapoor.Invoke("SetOption", $ADS_OPTION_PASSWORD_PORTNUMBER, $port)
$userDevKapoor.Invoke("SetOption", $ADS_OPTION_PASSWORD_METHOD, $ADS_PASSWORD_ENCODE_CLEAR)
$userDevKapoor.Invoke(”setPassword”,"Teraque")
$userDevKapoor.InvokeSet("accountDisabled", $false)
$userDevKapoor.SetInfo()
$roleTraders.member = $roleTraders.member + $userDevKapoor.distinguishedName
$roleTraders.SetInfo()

# Yusuf Jones
write-host "`tUser: Yusuf Jones"
$userYusufJones = $organizationalUnitAspenGroup.Create("user", "CN=Yusuf Jones")
$userYusufJones.Put("displayName", "Yusuf Jones")
$userYusufJones.Put("givenName", "Yusuf")
$userYusufJones.Put("name", "Yusuf Jones")
$userYusufJones.Put("sn", "Jones")
$userYusufJones.SetInfo()
$userYusufJones.AuthenticationType = $ADS_SECURE_AUTHENTICATION -bor $ADS_USE_SIGNING -bor $ADS_USE_SEALING
$userYusufJones.Invoke("SetOption", $ADS_OPTION_PASSWORD_PORTNUMBER, $port)
$userYusufJones.Invoke("SetOption", $ADS_OPTION_PASSWORD_METHOD, $ADS_PASSWORD_ENCODE_CLEAR)
$userYusufJones.Invoke(”setPassword”,"Teraque")
$userYusufJones.InvokeSet("accountDisabled", $false)
$userYusufJones.SetInfo()
$roleManagers.member = $roleManagers.member + $userYusufJones.distinguishedName
$roleManagers.SetInfo()
$roleTraders.member = $roleTraders.member + $userYusufJones.distinguishedName
$roleTraders.SetInfo()

# Hector Kurtz
write-host "`tUser: Hector Kurtz"
$userHectorKurtz = $organizationalUnitAspenGroup.Create("user", "CN=Hector Kurtz")
$userHectorKurtz.Put("displayName", "Hector Kurtz")
$userHectorKurtz.Put("givenName", "Hector")
$userHectorKurtz.Put("name", "Hector Kurtz")
$userHectorKurtz.Put("sn", "Kurtz")
$userHectorKurtz.SetInfo()
$userHectorKurtz.AuthenticationType = $ADS_SECURE_AUTHENTICATION -bor $ADS_USE_SIGNING -bor $ADS_USE_SEALING
$userHectorKurtz.Invoke("SetOption", $ADS_OPTION_PASSWORD_PORTNUMBER, $port)
$userHectorKurtz.Invoke("SetOption", $ADS_OPTION_PASSWORD_METHOD, $ADS_PASSWORD_ENCODE_CLEAR)
$userHectorKurtz.Invoke(”setPassword”,"Teraque")
$userHectorKurtz.InvokeSet("accountDisabled", $false)
$userHectorKurtz.SetInfo()
$roleTraders.member = $roleTraders.member + $userHectorKurtz.distinguishedName
$roleTraders.SetInfo()

# Juan Green
write-host "`tUser: Juan Green"
$userJuanGreen = $organizationalUnitAspenGroup.Create("user", "CN=Juan Green")
$userJuanGreen.Put("displayName", "Juan Green")
$userJuanGreen.Put("givenName", "Juan")
$userJuanGreen.Put("name", "Juan Green")
$userJuanGreen.Put("sn", "Green")
$userJuanGreen.SetInfo()
$userJuanGreen.AuthenticationType = $ADS_SECURE_AUTHENTICATION -bor $ADS_USE_SIGNING -bor $ADS_USE_SEALING
$userJuanGreen.Invoke("SetOption", $ADS_OPTION_PASSWORD_PORTNUMBER, $port)
$userJuanGreen.Invoke("SetOption", $ADS_OPTION_PASSWORD_METHOD, $ADS_PASSWORD_ENCODE_CLEAR)
$userJuanGreen.Invoke(”setPassword”,"Teraque")
$userJuanGreen.InvokeSet("accountDisabled", $false)
$userJuanGreen.SetInfo()
$roleManagers.member = $roleManagers.member + $userJuanGreen.distinguishedName
$roleManagers.SetInfo()

# Gary Stein
write-host "`tUser: Gary Stein"
$userGaryStein = $organizationalUnitAspenGroup.Create("user", "CN=Gary Stein")
$userGaryStein.Put("displayName", "Gary Stein")
$userGaryStein.Put("givenName", "Gary")
$userGaryStein.Put("name", "Gary Stein")
$userGaryStein.Put("sn", "Stein")
$userGaryStein.SetInfo()
$userGaryStein.AuthenticationType = $ADS_SECURE_AUTHENTICATION -bor $ADS_USE_SIGNING -bor $ADS_USE_SEALING
$userGaryStein.Invoke("SetOption", $ADS_OPTION_PASSWORD_PORTNUMBER, $port)
$userGaryStein.Invoke("SetOption", $ADS_OPTION_PASSWORD_METHOD, $ADS_PASSWORD_ENCODE_CLEAR)
$userGaryStein.Invoke(”setPassword”,"Teraque")
$userGaryStein.InvokeSet("accountDisabled", $false)
$userGaryStein.SetInfo()
$roleTraders.member = $roleTraders.member + $userGaryStein.distinguishedName
$roleTraders.SetInfo()

# Moorehead Brown
write-host "Organization: Moorehead Brown"
$organizationalUnitMooreheadBrown = $domain.Create("organizationalUnit", "OU=Moorehead Brown")
$organizationalUnitMooreheadBrown.SetInfo()

# Administrator
write-host "`tUser: Administrator"
$userAdministrator = $organizationalUnitMooreheadBrown.Create("user", "CN=Administrator")
$userAdministrator.Put("displayName", "Administrator")
$userAdministrator.Put("givenName", "Administrator")
$userAdministrator.Put("name", "Administrator")
$userAdministrator.SetInfo()
$userAdministrator.AuthenticationType = $ADS_SECURE_AUTHENTICATION -bor $ADS_USE_SIGNING -bor $ADS_USE_SEALING
$userAdministrator.Invoke("SetOption", $ADS_OPTION_PASSWORD_PORTNUMBER, $port)
$userAdministrator.Invoke("SetOption", $ADS_OPTION_PASSWORD_METHOD, $ADS_PASSWORD_ENCODE_CLEAR)
$userAdministrator.Invoke(”setPassword”,"Teraque")
$userAdministrator.InvokeSet("accountDisabled", $false)
$userAdministrator.SetInfo()
$roleTenantAdministrators.member = $roleTenantAdministrators.member + $userAdministrator.distinguishedName
$roleTenantAdministrators.SetInfo()

# Mitsuo Tanaka
write-host "`tUser: Mitsuo Tanaka"
$userMitsuoTanaka = $organizationalUnitMooreheadBrown.Create("user", "CN=Mitsuo Tanaka")
$userMitsuoTanaka.Put("displayName", "Mitsuo Tanaka")
$userMitsuoTanaka.Put("givenName", "Mitsuo")
$userMitsuoTanaka.Put("name", "Mitsuo Tanaka")
$userMitsuoTanaka.Put("sn", "Tanaka")
$userMitsuoTanaka.SetInfo()
$userMitsuoTanaka.AuthenticationType = $ADS_SECURE_AUTHENTICATION -bor $ADS_USE_SIGNING -bor $ADS_USE_SEALING
$userMitsuoTanaka.Invoke("SetOption", $ADS_OPTION_PASSWORD_PORTNUMBER, $port)
$userMitsuoTanaka.Invoke("SetOption", $ADS_OPTION_PASSWORD_METHOD, $ADS_PASSWORD_ENCODE_CLEAR)
$userMitsuoTanaka.Invoke(”setPassword”,"Teraque")
$userMitsuoTanaka.InvokeSet("accountDisabled", $false)
$userMitsuoTanaka.SetInfo()
$roleTraders.member = $roleTraders.member + $userMitsuoTanaka.distinguishedName
$roleTraders.SetInfo()

# Donovan Snow
write-host "`tUser: Donovan Snow"
$userDonovanSnow = $organizationalUnitMooreheadBrown.Create("user", "CN=Donovan Snow")
$userDonovanSnow.Put("displayName", "Donovan Snow")
$userDonovanSnow.Put("givenName", "Donovan")
$userDonovanSnow.Put("name", "Donovan Snow")
$userDonovanSnow.Put("sn", "Snow")
$userDonovanSnow.SetInfo()
$userDonovanSnow.AuthenticationType = $ADS_SECURE_AUTHENTICATION -bor $ADS_USE_SIGNING -bor $ADS_USE_SEALING
$userDonovanSnow.Invoke("SetOption", $ADS_OPTION_PASSWORD_PORTNUMBER, $port)
$userDonovanSnow.Invoke("SetOption", $ADS_OPTION_PASSWORD_METHOD, $ADS_PASSWORD_ENCODE_CLEAR)
$userDonovanSnow.Invoke(”setPassword”,"Teraque")
$userDonovanSnow.InvokeSet("accountDisabled", $false)
$userDonovanSnow.SetInfo()
$roleManagers.member = $roleManagers.member + $userDonovanSnow.distinguishedName
$roleManagers.SetInfo()

# Sergei Nabokov
write-host "`tUser: Sergei Nabokov"
$userSergeiNabokov = $organizationalUnitMooreheadBrown.Create("user", "CN=Sergei Nabokov")
$userSergeiNabokov.Put("displayName", "Sergei Nabokov")
$userSergeiNabokov.Put("givenName", "Sergei")
$userSergeiNabokov.Put("name", "Sergei Nabokov")
$userSergeiNabokov.Put("sn", "Nabokov")
$userSergeiNabokov.SetInfo()
$userSergeiNabokov.AuthenticationType = $ADS_SECURE_AUTHENTICATION -bor $ADS_USE_SIGNING -bor $ADS_USE_SEALING
$userSergeiNabokov.Invoke("SetOption", $ADS_OPTION_PASSWORD_PORTNUMBER, $port)
$userSergeiNabokov.Invoke("SetOption", $ADS_OPTION_PASSWORD_METHOD, $ADS_PASSWORD_ENCODE_CLEAR)
$userSergeiNabokov.Invoke(”setPassword”,"Teraque")
$userSergeiNabokov.InvokeSet("accountDisabled", $false)
$userSergeiNabokov.SetInfo()
$roleTraders.member = $roleTraders.member + $userSergeiNabokov.distinguishedName
$roleTraders.SetInfo()
$roleManagers.member = $roleManagers.member + $userSergeiNabokov.distinguishedName
$roleManagers.SetInfo()

# Elizabeth Johnson
write-host "`tUser: Elizabeth Johnson"
$userElizabethJohnson = $organizationalUnitMooreheadBrown.Create("user", "CN=Elizabeth Johnson")
$userElizabethJohnson.Put("displayName", "Elizabeth Johnson")
$userElizabethJohnson.Put("givenName", "Elizabeth")
$userElizabethJohnson.Put("name", "Elizabeth Johnson")
$userElizabethJohnson.Put("sn", "Johnson")
$userElizabethJohnson.SetInfo()
$userElizabethJohnson.AuthenticationType = $ADS_SECURE_AUTHENTICATION -bor $ADS_USE_SIGNING -bor $ADS_USE_SEALING
$userElizabethJohnson.Invoke("SetOption", $ADS_OPTION_PASSWORD_PORTNUMBER, $port)
$userElizabethJohnson.Invoke("SetOption", $ADS_OPTION_PASSWORD_METHOD, $ADS_PASSWORD_ENCODE_CLEAR)
$userElizabethJohnson.Invoke(”setPassword”,"Teraque")
$userElizabethJohnson.InvokeSet("accountDisabled", $false)
$userElizabethJohnson.SetInfo()
$roleTraders.member = $roleTraders.member + $userElizabethJohnson.distinguishedName
$roleTraders.SetInfo()
$roleManagers.member = $roleManagers.member + $userElizabethJohnson.distinguishedName
$roleManagers.SetInfo()
