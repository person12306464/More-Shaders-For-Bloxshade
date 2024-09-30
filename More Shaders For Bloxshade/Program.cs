using System.Security.Principal;

string root = AppDomain.CurrentDomain.BaseDirectory;

// Check if the program is running as admin

#pragma warning disable CA1416 // Validate platform compatibility
bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
#pragma warning restore CA1416 // Validate platform compatibility

if (isAdmin == false)
{
    Console.WriteLine("This program needs to be run as admin.");
    Console.WriteLine("It needs this to add files to the Ansel folder.");
    Console.WriteLine("Press Enter to close this.");
    Console.ReadLine();
    Environment.Exit(1);
}

// Auto updater
const int InstallerVersion = 211;
const string InstallerVersionString = "2.1.1";


// Download gist to determine what the latest version is
using (var client = new HttpClient())
{
    using (var s = client.GetStreamAsync("https://gist.githubusercontent.com/person12306464/43bfd48dcae5e6365a726985241099a2/raw/latestversion.txt"))
    {
        using (var fs = new FileStream(root + "latestversion.txt", FileMode.OpenOrCreate))
        {
            s.Result.CopyTo(fs);
        }
    }
}

// Make variables out of it
string[] versionLines = File.ReadAllLines(root + "latestversion.txt");

int latestVersionInt = Int32.Parse(versionLines[0]);

string latestVersionString = versionLines[1];

string latestVersionDownload = versionLines[2];

File.Delete(root + "latestversion.txt");


// Main auto update thing
if (latestVersionInt > InstallerVersion)
{
    bool confirmed = false;

    ConsoleKey response;

    Console.WriteLine("You have an outdated version!\n" +
                      "You currently have {0} and the latest version is {1}!", InstallerVersionString, latestVersionString);
    do
    {
        Console.Write("Do you wish to update? [y/n] ");
        response = Console.ReadKey(false).Key;   // true is intercept key (dont show), false is show
        if (response != ConsoleKey.Enter)
            Console.WriteLine();

    } while (response != ConsoleKey.Y && response != ConsoleKey.N);

    confirmed = response == ConsoleKey.Y;

    if (confirmed == true)
    {
        Console.WriteLine("Updating to the latest version...");


        // Check if the NewVersion directory exists already (It shouldn't but could do if there was an error before deleting it)
        if (Directory.Exists(root + "NewVersion"))
        {
            Directory.Delete(root + "NewVersion", true);
        }

        if (File.Exists(root + "NewVersion.zip"))
        {
            File.Delete(root + "NewVersion.zip");
        }


        // Download new version

        using (var client = new HttpClient())
        {
            using (var s = client.GetStreamAsync(latestVersionDownload))
            {
                using (var fs = new FileStream(root + "NewVersion.zip", FileMode.OpenOrCreate))
                {
                    s.Result.CopyTo(fs);
                }
            }
        }

        System.IO.Compression.ZipFile.ExtractToDirectory(root + "NewVersion.zip", root + "NewVersion");
        File.Delete(root + "NewVersion.zip");


        // Make bat file to delete the old version and then move the new version into the main directory and launch it
        // %~dp0 gets the root directory

        string batText = "@echo off\n" +
                         "timeout /T 3\n" +
                         "del \"%~dp0\\More Shaders For Bloxshade.exe\"\n" +
                         "move \"%~dp0\\NewVersion\\More Shaders For Bloxshade.exe\" \"%~dp0\"\n" +
                         "rmdir /s /q \"%~dp0\\NewVersion\"\n" +
                         "start \"\" \"" + "%~dp0\\More Shaders For Bloxshade.exe" + "\"\n" +
                         "del \"%~dp0\\update.bat\"";
        File.WriteAllText(root + "update.bat", batText);

        // Launch the bat file that was just createad
        System.Diagnostics.Process.Start(root + "update.bat");


        // Close old version
        Environment.Exit(0);
    }
}
Console.Clear();


// Start of actual program

Console.WriteLine("\n" +
              "  ███╗░░░███╗░█████╗░██████╗░███████╗  ░██████╗██╗░░██╗░█████╗░██████╗░███████╗██████╗░░██████╗\n" +
              "  ████╗░████║██╔══██╗██╔══██╗██╔════╝  ██╔════╝██║░░██║██╔══██╗██╔══██╗██╔════╝██╔══██╗██╔════╝\n" +
              "  ██╔████╔██║██║░░██║██████╔╝█████╗░░  ╚█████╗░███████║███████║██║░░██║█████╗░░██████╔╝╚█████╗░\n" +
              "  ██║╚██╔╝██║██║░░██║██╔══██╗██╔══╝░░  ░╚═══██╗██╔══██║██╔══██║██║░░██║██╔══╝░░██╔══██╗░╚═══██╗\n" +
              "  ██║░╚═╝░██║╚█████╔╝██║░░██║███████╗  ██████╔╝██║░░██║██║░░██║██████╔╝███████╗██║░░██║██████╔╝\n" +
              "  ╚═╝░░░░░╚═╝░╚════╝░╚═╝░░╚═╝╚══════╝  ╚═════╝░╚═╝░░╚═╝╚═╝░░╚═╝╚═════╝░╚══════╝╚═╝░░╚═╝╚═════╝░\n" +
              "" +
              "  ███████╗░█████╗░██████╗░  ██████╗░██╗░░░░░░█████╗░██╗░░██╗░██████╗██╗░░██╗░█████╗░██████╗░███████╗\n" +
              "  ██╔════╝██╔══██╗██╔══██╗  ██╔══██╗██║░░░░░██╔══██╗╚██╗██╔╝██╔════╝██║░░██║██╔══██╗██╔══██╗██╔════╝\n" +
              "  █████╗░░██║░░██║██████╔╝  ██████╦╝██║░░░░░██║░░██║░╚███╔╝░╚█████╗░███████║███████║██║░░██║█████╗░░\n" +
              "  ██╔══╝░░██║░░██║██╔══██╗  ██╔══██╗██║░░░░░██║░░██║░██╔██╗░░╚═══██╗██╔══██║██╔══██║██║░░██║██╔══╝░░\n" +
              "  ██║░░░░░╚█████╔╝██║░░██║  ██████╦╝███████╗╚█████╔╝██╔╝╚██╗██████╔╝██║░░██║██║░░██║██████╔╝███████╗\n" +
              "  ╚═╝░░░░░░╚════╝░╚═╝░░╚═╝  ╚═════╝░╚══════╝░╚════╝░╚═╝░░╚═╝╚═════╝░╚═╝░░╚═╝╚═╝░░╚═╝╚═════╝░╚══════╝");

Console.WriteLine("\n{0}\n\nPress Enter to start the download.", InstallerVersionString);

Console.ReadLine();


Console.WriteLine("\nDownloading shaders...");

string Ansel = "C:\\Program Files\\NVIDIA Corporation\\Ansel\\";

string shaderDir = Ansel + "shaders\\msfb_shaders-8309018d0609f836a1ad720f758685c9b95e52f1\\";


//Check if any of the files exists, if it does it gets deleted

// I know this is alot of manual work i don't care to make something dynamically do this
// Like it works, alright
if (File.Exists(Ansel + "shaders.zip"))
{
    File.Delete(Ansel + "shaders.zip");
}

if (Directory.Exists(Ansel + "shaders"))
{
    Directory.Delete(Ansel + "shaders", true);
}

if (File.Exists(Ansel + "AgX.fx"))
{
    File.Delete(Ansel + "AgX.fx");
}

if (File.Exists(Ansel + "AgX-default_contrast.lut.png"))
{
    File.Delete(Ansel + "AgX-default_contrast.lut.png");
}

if (File.Exists(Ansel + "dh_rtgi.fx"))
{
    File.Delete(Ansel + "dh_rtgi.fx");
}

if (File.Exists(Ansel + "Limbo_Mod.fx"))
{
    File.Delete(Ansel + "Limbo_Mod.fx");
}

if (File.Exists(Ansel + "SnowScape.fx"))
{
    File.Delete(Ansel + "SnowScape.fx");
}

if (File.Exists(Ansel + "Deband.fx"))
{
    File.Delete(Ansel + "Deband.fx");
}

if (File.Exists(Ansel + "DiffuseGlow.fx"))
{
    File.Delete(Ansel + "DiffuseGlow.fx");
}

if (File.Exists(Ansel + "FGFXLargeScalePerceptualObscuranceIrradiance.fx"))
{
    File.Delete(Ansel + "FGFXLargeScalePerceptualObscuranceIrradiance.fx");
}

if (File.Exists(Ansel + "GILT.fx"))
{
    File.Delete(Ansel + "GILT.fx");
}

if (File.Exists(Ansel + "GILT1.fx"))
{
    File.Delete(Ansel + "GILT1.fx");
}

if (File.Exists(Ansel + "GILT2.fx"))
{
    File.Delete(Ansel + "GILT2.fx");
}

if (File.Exists(Ansel + "GILT3.fx"))
{
    File.Delete(Ansel + "GILT3.fx");
}

if (File.Exists(Ansel + "GILT4.fx"))
{
    File.Delete(Ansel + "GILT4.fx");
}

if (File.Exists(Ansel + "GILT5.fx"))
{
    File.Delete(Ansel + "GILT5.fx");
}

if (File.Exists(Ansel + "GILT4_NoNoise.fx"))
{
    File.Delete(Ansel + "GILT4_NoNoise.fx");
}

if (File.Exists(Ansel + "Glamayre_Fast_Effects.fx"))
{
    File.Delete(Ansel + "Glamayre_Fast_Effects.fx");
}

if (File.Exists(Ansel + "Bumpmapping.fx"))
{
    File.Delete(Ansel + "Bumpmapping.fx");
}

if (File.Exists(Ansel + "Deblur.fx"))
{
    File.Delete(Ansel + "Deblur.fx");
}

if (File.Exists(Ansel + "DeblurUpscaler.fx"))
{
    File.Delete(Ansel + "DeblurUpscaler.fx");
}

if (File.Exists(Ansel + "HBAO.fx"))
{
    File.Delete(Ansel + "HBAO.fx");
}

if (File.Exists(Ansel + "Low_sats_check.fx"))
{
    File.Delete(Ansel + "Low_sats_check.fx");
}

if (File.Exists(Ansel + "Low_saturation_check.fx"))
{
    File.Delete(Ansel + "Low_saturation_check.fx");
}

if (File.Exists(Ansel + "White Point 2D.fx"))
{
    File.Delete(Ansel + "White Point 2D.fx");
}

if (File.Exists(Ansel + "White Point RGB 2D.fx"))
{
    File.Delete(Ansel + "White Point RGB 2D.fx");
}

if (File.Exists(Ansel + "White Point_No_Debug.fx"))
{
    File.Delete(Ansel + "White Point_No_Debug.fx");
}

if (File.Exists(Ansel + "xy Primaries 2D.fx"))
{
    File.Delete(Ansel + "xy Primaries 2D.fx");
}

if (File.Exists(Ansel + "Y_gamma.fx"))
{
    File.Delete(Ansel + "Y_gamma.fx");
}

if (File.Exists(Ansel + "Y_Invert.fx"))
{
    File.Delete(Ansel + "Y_Invert.fx");
}

if (File.Exists(Ansel + "256color.fx"))
{
    File.Delete(Ansel + "256color.fx");
}

if (File.Exists(Ansel + "ASSMAA.fx"))
{
    File.Delete(Ansel + "ASSMAA.fx");
}

if (File.Exists(Ansel + "FSMAA.fx"))
{
    File.Delete(Ansel + "FSMAA.fx");
}

if (File.Exists(Ansel + "HQAA.fx"))
{
    File.Delete(Ansel + "HQAA.fx");
}

if (File.Exists(Ansel + "HQAALite.fx"))
{
    File.Delete(Ansel + "HQAALite.fx");
}

if (File.Exists(Ansel + "ImageSoften.fx"))
{
    File.Delete(Ansel + "ImageSoften.fx");
}

if (File.Exists(Ansel + "QXAA.fx"))
{
    File.Delete(Ansel + "QXAA.fx");
}

if (File.Exists(Ansel + "SmartBrightnessBooster.fx"))
{
    File.Delete(Ansel + "SmartBrightnessBooster.fx");
}

if (File.Exists(Ansel + "STAA.fx"))
{
    File.Delete(Ansel + "STAA.fx");
}

if (File.Exists(Ansel + "TSMAA.fx"))
{
    File.Delete(Ansel + "TSMAA.fx");
}

if (File.Exists(Ansel + "TSMAA2.fx"))
{
    File.Delete(Ansel + "TSMAA2.fx");
}

if (File.Exists(Ansel + "XHQAA.fx"))
{
    File.Delete(Ansel + "XHQAA.fx");
}

if (File.Exists(Ansel + "MC_SSAO.fx"))
{
    File.Delete(Ansel + "MC_SSAO.fx");
}

if (File.Exists(Ansel + "MC_Tonemap.fx"))
{
    File.Delete(Ansel + "MC_Tonemap.fx");
}

if (File.Exists(Ansel + "MC_TonemapHDR.fx"))
{
    File.Delete(Ansel + "MC_TonemapHDR.fx");
}

if (File.Exists(Ansel + "NGLighting_specular.fx"))
{
    File.Delete(Ansel + "NGLighting_specular.fx");
}

if (File.Exists(Ansel + "NGLighting-Configs_specular.fxh"))
{
    File.Delete(Ansel + "NGLighting-Configs_specular.fxh");
}

if (File.Exists(Ansel + "NGLighting-Shader_specular.fxh"))
{
    File.Delete(Ansel + "NGLighting-Shader_specular.fxh");
}

if (File.Exists(Ansel + "NGLightingUI_specular.fxh"))
{
    File.Delete(Ansel + "NGLightingUI_specular.fxh");
}

if (File.Exists(Ansel + "Volumetric Fog V2.0.fx"))
{
    File.Delete(Ansel + "Volumetric Fog V2.0.fx");
}

if (File.Exists(Ansel + "Reinhard.fx"))
{
    File.Delete(Ansel + "Reinhard.fx");
}

if (File.Exists(Ansel + "5XBR_NoBlend.fx"))
{
    File.Delete(Ansel + "5XBR_NoBlend.fx");
}

if (File.Exists(Ansel + "ShaderFastMathLib.h"))
{
    File.Delete(Ansel + "ShaderFastMathLib.h");
}

if (File.Exists(Ansel + "YASSGI_bleu.png"))
{
    File.Delete(Ansel + "YASSGI_bleu.png");
}

if (File.Exists(Ansel + "YASSGI_old_tracer.fx"))
{
    File.Delete(Ansel + "YASSGI_old_tracer.fx");
}

if (File.Exists(Ansel + "ZN_GI.fx"))
{
    File.Delete(Ansel + "ZN_GI.fx");
}

if (File.Exists(Ansel + "ZNbluenoise512.png"))
{
    File.Delete(Ansel + "ZNbluenoise512.png");
}

if (File.Exists(Ansel + "GloomAO.fx"))
{
    File.Delete(Ansel + "GloomAO.fx");
}

if (File.Exists(Ansel + "Droste.fx"))
{
    File.Delete(Ansel + "Droste.fx");
}

if (File.Exists(Ansel + "LongExposure.fx"))
{
    File.Delete(Ansel + "LongExposure.fx");
}

if (File.Exists(Ansel + "RealLongExposure.fx"))
{
    File.Delete(Ansel + "RealLongExposure.fx");
}

if (File.Exists(Ansel + "FGFXFastCascadedSeparableBlur16X.fx"))
{
    File.Delete(Ansel + "FGFXFastCascadedSeparableBlur16X.fx");
}

if (File.Exists(Ansel + "PerfectPerspective.fx"))
{
    File.Delete(Ansel + "PerfectPerspective.fx");
}

if (File.Exists(Ansel + "Particles.fx"))
{
    File.Delete(Ansel + "Particles.fx");
}

if (File.Exists(Ansel + "AtmosphericDensity.fx"))
{
    File.Delete(Ansel + "AtmosphericDensity.fx");
}

if (File.Exists(Ansel + "MShaders.zip"))
{
    File.Delete(Ansel + "MShaders.zip");
}

if (Directory.Exists(Ansel + "MShaders"))
{
    Directory.Delete(Ansel + "MShaders", true);
}

if (Directory.Exists(Ansel + "Include"))
{
    Directory.Delete(Ansel + "Include", true);
}






// Download shaders
using (var client = new HttpClient())
{
    using (var s = client.GetStreamAsync("https://github.com/person12306464/msfb_shaders/archive/8309018d0609f836a1ad720f758685c9b95e52f1.zip"))
    {
        using (var fs = new FileStream(Ansel + "shaders.zip", FileMode.OpenOrCreate))
        {
            s.Result.CopyTo(fs);
        }
    }
}

Console.WriteLine("Installing shaders...");


// Extract
System.IO.Compression.ZipFile.ExtractToDirectory(Ansel + "shaders.zip", Ansel + "shaders");
File.Delete(Ansel + "shaders.zip");



// Move all of them
File.Move(shaderDir + "AgX\\AgX.fx", Ansel + "AgX.fx");
File.Move(shaderDir + "AgX\\AgX-default_contrast.lut.png", Ansel + "AgX-default_contrast.lut.png");
File.Move(shaderDir + "AlucardDH\\dh_rtgi.fx", Ansel + "dh_rtgi.fx");
File.Move(shaderDir + "BlueSkyDefender\\Limbo_Mod.fx", Ansel + "Limbo_Mod.fx");
File.Move(shaderDir + "BlueSkyDefender\\SnowScape.fx", Ansel + "SnowScape.fx");
File.Move(shaderDir + "deband\\Deband.fx", Ansel + "Deband.fx");
File.Move(shaderDir + "DiffuseGlow\\DiffuseGlow.fx", Ansel + "DiffuseGlow.fx");
File.Move(shaderDir + "FGFX\\FGFXLargeScalePerceptualObscuranceIrradiance.fx", Ansel + "FGFXLargeScalePerceptualObscuranceIrradiance.fx");
File.Move(shaderDir + "GILT\\GILT.fx", Ansel + "GILT.fx");
File.Move(shaderDir + "GILT\\GILT1.fx", Ansel + "GILT1.fx");
File.Move(shaderDir + "GILT\\GILT2.fx", Ansel + "GILT2.fx");
File.Move(shaderDir + "GILT\\GILT3.fx", Ansel + "GILT3.fx");
File.Move(shaderDir + "GILT\\GILT4.fx", Ansel + "GILT4.fx");
File.Move(shaderDir + "GILT\\GILT5.fx", Ansel + "GILT5.fx");
File.Move(shaderDir + "GILT\\GILT4_NoNoise.fx", Ansel + "GILT4_NoNoise.fx");
File.Move(shaderDir + "Glamayre\\Glamayre_Fast_Effects.fx", Ansel + "Glamayre_Fast_Effects.fx");
File.Move(shaderDir + "guestrr\\Bumpmapping.fx", Ansel + "Bumpmapping.fx");
File.Move(shaderDir + "guestrr\\Deblur.fx", Ansel + "Deblur.fx");
File.Move(shaderDir + "guestrr\\DeblurUpscaler.fx", Ansel + "DeblurUpscaler.fx");
File.Move(shaderDir + "HBAO\\HBAO.fx", Ansel + "HBAO.fx");
File.Move(shaderDir + "HLSL-Reshade-colour-remappers\\Low_sats_check.fx", Ansel + "Low_sats_check.fx");
File.Move(shaderDir + "HLSL-Reshade-colour-remappers\\Low_saturation_check.fx", Ansel + "Low_saturation_check.fx");
File.Move(shaderDir + "HLSL-Reshade-colour-remappers\\White Point 2D.fx", Ansel + "White Point 2D.fx");
File.Move(shaderDir + "HLSL-Reshade-colour-remappers\\White Point RGB 2D.fx", Ansel + "White Point RGB 2D.fx");
File.Move(shaderDir + "HLSL-Reshade-colour-remappers\\White Point_No_Debug.fx", Ansel + "White Point_No_Debug.fx");
File.Move(shaderDir + "HLSL-Reshade-colour-remappers\\xy Primaries 2D.fx", Ansel + "xy Primaries 2D.fx");
File.Move(shaderDir + "HLSL-Reshade-colour-remappers\\Y_gamma.fx", Ansel + "Y_gamma.fx");
File.Move(shaderDir + "HLSL-Reshade-colour-remappers\\Y_Invert.fx", Ansel + "Y_Invert.fx");
File.Move(shaderDir + "lordbean\\256color.fx", Ansel + "256color.fx");
File.Move(shaderDir + "lordbean\\ASSMAA.fx", Ansel + "ASSMAA.fx");
File.Move(shaderDir + "lordbean\\FSMAA.fx", Ansel + "FSMAA.fx");
File.Move(shaderDir + "lordbean\\HQAA.fx", Ansel + "HQAA.fx");
File.Move(shaderDir + "lordbean\\HQAALite.fx", Ansel + "HQAALite.fx");
File.Move(shaderDir + "lordbean\\ImageSoften.fx", Ansel + "ImageSoften.fx");
File.Move(shaderDir + "lordbean\\QXAA.fx", Ansel + "QXAA.fx");
File.Move(shaderDir + "lordbean\\SmartBrightnessBooster.fx", Ansel + "SmartBrightnessBooster.fx");
File.Move(shaderDir + "lordbean\\STAA.fx", Ansel + "STAA.fx");
File.Move(shaderDir + "lordbean\\TSMAA.fx", Ansel + "TSMAA.fx");
File.Move(shaderDir + "lordbean\\TSMAA2.fx", Ansel + "TSMAA2.fx");
File.Move(shaderDir + "lordbean\\XHQAA.fx", Ansel + "XHQAA.fx");
File.Move(shaderDir + "MC\\MC_SSAO.fx", Ansel + "MC_SSAO.fx");
File.Move(shaderDir + "MC\\MC_Tonemap.fx", Ansel + "MC_Tonemap.fx");
File.Move(shaderDir + "MC\\MC_TonemapHDR.fx", Ansel + "MC_TonemapHDR.fx");
File.Move(shaderDir + "NiceGuy\\NGLighting_specular.fx", Ansel + "NGLighting_specular.fx");
File.Move(shaderDir + "NiceGuy\\NGLighting-Configs_specular.fxh", Ansel + "NGLighting-Configs_specular.fxh");
File.Move(shaderDir + "NiceGuy\\NGLighting-Shader_specular.fxh", Ansel + "NGLighting-Shader_specular.fxh");
File.Move(shaderDir + "NiceGuy\\NGLightingUI_specular.fxh", Ansel + "NGLightingUI_specular.fxh");
File.Move(shaderDir + "NiceGuy\\Volumetric Fog V2.0.fx", Ansel + "Volumetric Fog V2.0.fx");
File.Move(shaderDir + "Reinhard\\Reinhard.fx", Ansel + "Reinhard.fx");
File.Move(shaderDir + "xbr\\5XBR_NoBlend.fx", Ansel + "5XBR_NoBlend.fx");
File.Move(shaderDir + "YASSGI\\ShaderFastMathLib.h", Ansel + "ShaderFastMathLib.h");
File.Move(shaderDir + "YASSGI\\YASSGI_bleu.png", Ansel + "YASSGI_bleu.png");
File.Move(shaderDir + "YASSGI\\YASSGI_old_tracer.fx", Ansel + "YASSGI_old_tracer.fx");
File.Move(shaderDir + "Zenteon\\ZN_GI.fx", Ansel + "ZN_GI.fx");
File.Move(shaderDir + "Zenteon\\ZNbluenoise512.png", Ansel + "ZNbluenoise512.png");
File.Move(shaderDir + "CobraFX\\Droste.fx", Ansel + "Droste.fx");
File.Move(shaderDir + "CobraFX\\LongExposure.fx", Ansel + "LongExposure.fx");
File.Move(shaderDir + "CobraFX\\RealLongExposure.fx", Ansel + "RealLongExposure.fx");
File.Move(shaderDir + "FGFX\\FGFXFastCascadedSeparableBlur16X.fx", Ansel + "FGFXFastCascadedSeparableBlur16X.fx");
File.Move(shaderDir + "Fubax\\PerfectPerspective.fx", Ansel + "PerfectPerspective.fx");

Directory.Delete(Ansel + "shaders", true);

// That took a while to do...


static void lineChanger(string newText, string fileName, int line_to_edit)
{
    string[] arrLine = File.ReadAllLines(fileName);
    arrLine[line_to_edit - 1] = newText;
    File.WriteAllLines(fileName, arrLine);
}

static void lineAdder(string fileName, int line)
{
    var allLines = File.ReadAllLines(fileName).ToList();
    allLines.Insert(line, "");
    File.WriteAllLines(fileName, allLines.ToArray());
}



// Now for GloomAO..

Console.WriteLine("Downloading GloomAO...");
using (var client = new HttpClient())
{
    using (var s = client.GetStreamAsync("https://raw.githubusercontent.com/BlueSkyDefender/AstrayFX/910e3213a846b34dd65d94e84b61b61fca69dd6d/Shaders/GloomAO.fx"))
    {
        using (var fs = new FileStream(Ansel + "GloomAO.fx", FileMode.OpenOrCreate))
        {
            s.Result.CopyTo(fs);
        }
    }
}


// Start modifying GloomAO

Console.WriteLine("Patching GloomAO...");


lineChanger("", Ansel + "GloomAO.fx", 467);

lineChanger("		zBuffer = rcp(Z.y * C.y + C.x);", Ansel + "GloomAO.fx", 708);
lineChanger("		zBufferWH = Far * NearWH / (Far + zB.y * (NearWH - Far));", Ansel + "GloomAO.fx", 709);

lineChanger("		zBuffer = rcp(Z.x * C.y + C.x);", Ansel + "GloomAO.fx", 713);
lineChanger("		zBufferWH = Far * NearWH / (Far + zB.x * (NearWH - Far));", Ansel + "GloomAO.fx", 714);

lineChanger("", Ansel + "GloomAO.fx", 1009);
lineChanger("", Ansel + "GloomAO.fx", 1010);
lineChanger("", Ansel + "GloomAO.fx", 1011);
lineChanger("", Ansel + "GloomAO.fx", 1012);
lineChanger("", Ansel + "GloomAO.fx", 1013);


// Particles

Console.WriteLine("Downloading Particles...");
using (var client = new HttpClient())
{
    using (var s = client.GetStreamAsync("https://raw.githubusercontent.com/crosire/reshade-shaders/6b452c4a101ccb228c4986560a51c571473c517b/ShadersAndTextures/Particles.fx"))
    {
        using (var fs = new FileStream(Ansel + "Particles.fx", FileMode.OpenOrCreate))
        {
            s.Result.CopyTo(fs);
        }
    }
}


Console.WriteLine("Patching Particles...");

lineAdder(Ansel + "Particles.fx", 180);
lineAdder(Ansel + "Particles.fx", 180);
lineAdder(Ansel + "Particles.fx", 180);
lineAdder(Ansel + "Particles.fx", 180);
lineAdder(Ansel + "Particles.fx", 180);
lineAdder(Ansel + "Particles.fx", 180);


lineChanger("float getBUFFER_HEIGHT()", Ansel + "Particles.fx", 182);
lineChanger("{", Ansel + "Particles.fx", 183);
lineChanger("	return BUFFER_HEIGHT;", Ansel + "Particles.fx", 184);
lineChanger("}", Ansel + "Particles.fx", 185);

lineChanger("	#if (getBUFFER_HEIGHT() <= 720)", Ansel + "Particles.fx", 189);
lineChanger("	#elif (getBUFFER_HEIGHT() <= 1080)", Ansel + "Particles.fx", 191);
lineChanger("	#elif (getBUFFER_HEIGHT() <= 1440)", Ansel + "Particles.fx", 193);
lineChanger("	#elif (getBUFFER_HEIGHT() <= 2160)", Ansel + "Particles.fx", 195);



// Atmospheric Density

Console.WriteLine("Downloading AtmosphericDensity...");
using (var client = new HttpClient())
{
    using (var s = client.GetStreamAsync("https://github.com/TreyM/MShaders-1/archive/d38b1af92d047b96819c898400919798e265c1cd.zip"))
    {
        using (var fs = new FileStream(Ansel + "MShaders.zip", FileMode.OpenOrCreate))
        {
            s.Result.CopyTo(fs);
        }
    }
}

System.IO.Compression.ZipFile.ExtractToDirectory(Ansel + "MShaders.zip", Ansel + "MShaders");
File.Delete(Ansel + "MShaders.zip");
File.Move(Ansel + "MShaders\\MShaders-1-d38b1af92d047b96819c898400919798e265c1cd\\Shaders\\MShaders\\AtmosphericDensity.fx", Ansel + "AtmosphericDensity.fx");
Directory.Move(Ansel + "MShaders\\MShaders-1-d38b1af92d047b96819c898400919798e265c1cd\\Shaders\\MShaders\\Include", Ansel + "Include");
Directory.Delete(Ansel + "MShaders", true);



Console.WriteLine("Patching AtmosphericDensity...");


lineChanger("    #define ENABLE_MISC_CONTROLS 1", Ansel + "AtmosphericDensity.fx", 53);
lineChanger("    float  depth, sky;", Ansel + "AtmosphericDensity.fx", 486);
lineChanger("", Ansel + "AtmosphericDensity.fx", 493);
lineChanger("", Ansel + "AtmosphericDensity.fx", 526);
lineChanger("", Ansel + "AtmosphericDensity.fx", 527);
lineChanger("", Ansel + "AtmosphericDensity.fx", 528);
lineChanger("", Ansel + "AtmosphericDensity.fx", 546);

lineChanger("UI_COMBO (AUTO_COLOR, \"Fog Color Mode\", \"\", 0, 1,", Ansel + "AtmosphericDensity.fx", 66);



// MXAO IL

Console.WriteLine("Downloading qUINT_mxao_il...");
using (var client = new HttpClient())
{
    using (var s = client.GetStreamAsync("https://raw.githubusercontent.com/martymcmodding/qUINT/98fed77b26669202027f575a6d8f590426c21ebd/Shaders/qUINT_mxao.fx"))
    {
        using (var fs = new FileStream(Ansel + "qUINT_mxao_il.fx", FileMode.OpenOrCreate))
        {
            s.Result.CopyTo(fs);
        }
    }
}

lineChanger(" #define MXAO_ENABLE_IL			1	//[0 or 1]	    Enables Indirect Lighting calculation. Will cause a major fps hit.", Ansel + "AtmosphericDensity.fx", 31);







// This is a checkmark, it looks kinda broken in the code but when running the program it looks good
Console.WriteLine("\n        _ \n" +
                  "       / /\n" +
                  "      / / \n" +
                  " /\\  / /  \n" +
                  " \\ \\/ /   \n" +
                  "  \\  /    \n" +
                  "   \\/     \n");


Console.WriteLine("Successfully installed the following shaders:\n\n" +
                  "AgX.fx (By MrLixm)\n" +
                  "dh_rtgi.fx (By AlucardDH)\n" +
                  "Limbo_Mod.fx (By BlueSkyDefender, License: https://creativecommons.org/licenses/by/2.0/)\n" +
                  "SnowScape.fx (By BlueSkyDefender, License: https://creativecommons.org/licenses/by/2.0/)\n" +
                  "Deband.fx (By haasn, modified and optimized for ReShade by JPulowski.)\n" +
                  "GILT.fx (By Extravi) (GILT 1-5 are test versions)\n" +
                  "Glamayre_Fast_Effects.fx (By Robert Jessop)\n" +
                  "Bumpmapping.fx (By guestrr, patched by Person123)\n" +
                  "Deblur.fx (By guestrr)\n" +
                  "DeblurUpscaler.fx (guestrr)\n" +
                  "HBAO.fx (By Ethatron, tomerk and Marty McFly, Ported from ReShade Framework to ReShade 3.0+ by mbah.primbon)\n" +
                  "Low_sats_check.fx (By crabshank)\n" +
                  "Low_saturation_check.fx (By crabshank)\n" +
                  "White Point 2D.fx (By crabshank)\n" +
                  "White Point RGB 2D.fx (By crabshank)\n" +
                  "White Point_No_Debug.fx (By crabshank)\n" +
                  "xy Primaries 2D.fx (By crabshank)\n" +
                  "Y_gamma.fx (By crabshank)\n" +
                  "Y_Invert.fx (By crabshank)\n" +
                  "256color.fx (By lordbean)\n" +
                  "ASSMAA.fx (By lordbean)\n" +
                  "FSMAA.fx (By lordbean)\n" +
                  "HQAA.fx (By lordbean)\n" +
                  "HQAALite.fx (By lordbean)\n" +
                  "ImageSoften.fx (By lordbean)\n" +
                  "QXAA.fx (By lordbean)\n" +
                  "SmartBrightnessBooster.fx (By lordbean)\n" +
                  "STAA.fx (By lordbean)\n" +
                  "TSMAA.fx (By lordbean)\n" +
                  "TSMAA2.fx (By lordbean)\n" +
                  "XHQAA.fx (By lordbean)\n" +
                  "MC_SSAO.fx (By Constantine 'MadCake' Rudenko, License: https://creativecommons.org/licenses/by/4.0/)\n" +
                  "MC_Tonemap.fx (By Constantine 'MadCake' Rudenko, License: https://creativecommons.org/licenses/by/4.0/) (Has been modified)\n" +
                  "MC_TonemapHDR.fx (By Constantine 'MadCake' Rudenko, License: https://creativecommons.org/licenses/by/4.0/) (Has been modified)\n" +
                  "Reinhard.fx (Original code by Marty McFly, Amateur port by Insomnia)\n" +
                  "5XBR_NoBlend.fx (By Hyllian, Ported by spiderh @2018)\n" +
                  "YASSGI_old_tracer.fx (By Pentalimbed)\n" +
                  "ZN_GI.fx (By Zenteon, patched by Sync)\n" +
                  "GloomAO.fx (By BlueSkyDefender, License: https://creativecommons.org/licenses/by-nd/4.0, patched by Sync)\n" +
                  "Droste.fx (By SirCobra)\n" +
                  "LongExposure.fx (By SirCobra)\n" +
                  "FGFXFastCascadedSeparableBlur16X.fx (By Alex Tuduran, patched by Person123)\n" +
                  "PerfectPerspective.fx (By Fubaxiusz, License: http://creativecommons.org/licenses/by-sa/3.0/, patched by Person123) (Has been modified)\n" +
                  "Particles.fx (By BlueSkyDefender, License: https://creativecommons.org/licenses/by-nd/4.0/, patched by Person123)\n" +
                  "qUINT_mxao_il.fx (By Marty McFly, MXAO with IL enabled.)\n" +
                  "\n" +
                  "Highlights:\n\n" +
                  "NGLighting_specular.fx (By NiceGuy, patched by Extravi, modified by Person123)\n" +
                  "Ray traced reflections, for free.\n\n" +
                  "FGFXLargeScalePerceptualObscuranceIrradiance.fx (By Alex Tuduran, patched by Person123)\n" +
                  "Adds large scale perceptual obscurance irradiance, ok I'm not exactly sure what this does but it's cool right?\n\n" +
                  "Volumetric Fog V2.0.fx (By NiceGuy, patched by Person123)\n" +
                  "Adds some kind of fake volumetric fog by blurring the background and blending it and doing a bunch of other things, it looks really cool actually.\n\n" +
                  "AtmosphericDensity.fx (By TreyM, patched by Sync)\n" +
                  "Good fog shader, only \"Exact Fog Color\" works correctly in Ansel, very broken in general but pair it with Volumetric Fog V2 and you got some good fog.\n" +
                  "If this shader worked fully correctly it would've been a replacment for Volumetric Fog V2 but it's more of a replacment for AdaptiveFog in Ansel.\n\n" +
                  "RealLongExposure.fx (By SirCobra)\n" +
                  "Long Exposure shader, toggle the UI option on and it will blend the frames together for the defined amount of time.\n\n" +
                  "DiffuseGlow.fx (By Ioxa)\n" +
                  "I can't really explain this one, just try it yourself, it's cool.\n\n");


Console.WriteLine("Successfully installed all shaders above. ^^^\n\n" +
                  "        _ \n" +
                  "       / /\n" +
                  "      / / \n" +
                  " /\\  / /  \n" +
                  " \\ \\/ /   \n" +
                  "  \\  /    \n" +
                  "   \\/     \n");

Console.WriteLine("Press enter to close this.");
Console.ReadLine();
