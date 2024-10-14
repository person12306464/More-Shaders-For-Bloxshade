using System.Security.Principal;


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




////////////////////////////////////////////////////////////////////////////////////////////////////
//                                             Common                                             //
//                                                                                                //


string root = AppDomain.CurrentDomain.BaseDirectory; // Gets the directory the executable is in

// Console.WriteLine(); // Print
// Console.ReadKey(); // Basically like a pause thing until you press a key
// Console.Readline(); // Same as the other one but you enter a line instead and the program continues when you press enter


// File.Move("path\\file.txt", "newpath\file.txt");
// File.Delete("path\\file.txt");

// Directory.CreateDirectory("path\\dirname");
// Directory.Delete("path\\dirname", true);  // The "true" at the end is if child directories and files should be deleted too

// Environment.Exit(0); // Closes the program with the specified exit code. 0 is normal and anything else is if something went wrong



// Extract zip
static void Extract(string inputZip, string outputDir)
{
    System.IO.Compression.ZipFile.ExtractToDirectory(inputZip, outputDir);
}


// Change line
static void ChangeLine(string newText, string fileName, int line_to_edit)
{
    string[] arrLine = File.ReadAllLines(fileName);
    arrLine[line_to_edit - 1] = newText;
    File.WriteAllLines(fileName, arrLine);
}


// Add line
static void AddLine(string fileName, int line)
{
    var allLines = File.ReadAllLines(fileName).ToList();
    allLines.Insert(line, "");
    File.WriteAllLines(fileName, allLines.ToArray());
}


// If file exists delete it
static void IfFileExistsDelete(string file)
{
    if (File.Exists(file))
    {
        File.Delete(file);
    }
}


// If directory exists delete it
static void IfDirectoryExistsDelete(string dir)
{
    if (Directory.Exists(dir))
    {
        Directory.Delete(dir, true);
    }
}


// Download file
static void Download(string url, string output)
{
    using (var client = new HttpClient())
    {
        using (var s = client.GetStreamAsync(url))
        {
            using (var fs = new FileStream(output, FileMode.OpenOrCreate))
            {
                s.Result.CopyTo(fs);
            }
        }
    }
}



// Yes or no choice prompt
static bool Choice(string text)
{
    bool confirmed = false;

    ConsoleKey response;

    do
    {
        Console.Write(text + " [y/n] ");
        response = Console.ReadKey(false).Key;   // true is intercept key (dont show), false is show
        if (response != ConsoleKey.Enter)
            Console.WriteLine();

    } while (response != ConsoleKey.Y && response != ConsoleKey.N);

    confirmed = response == ConsoleKey.Y;

    return confirmed;
}



//                                                                                                //
//                                                                                                //
////////////////////////////////////////////////////////////////////////////////////////////////////





// Auto updater
const int InstallerVersion = 2200;
const string InstallerVersionString = "2.2.0.0";


// Download gist to determine what the latest version is

Download("https://gist.githubusercontent.com/person12306464/47ba19202252b0b11c9b4ac47846ab7c/raw/latestversion.txt", root + "latestversion.txt");


// Make variables out of it
string[] versionLines = File.ReadAllLines(root + "latestversion.txt");

int latestVersionInt = Int32.Parse(versionLines[0]);

string latestVersionString = versionLines[1];

string latestVersionDownload = versionLines[2];

File.Delete(root + "latestversion.txt");


// Main auto update thing
if (latestVersionInt > InstallerVersion)
{

    Console.WriteLine("You have an outdated version!\n" +
                      "You currently have {0} and the latest version is {1}!", InstallerVersionString, latestVersionString);


    bool result = Choice("Do you wish to update?");



    if (result == true)
    {
        Console.WriteLine("Updating to the latest version...");


        // Check if the NewVersion directory exists already (It shouldn't but could do if there was an error before deleting it)

        IfDirectoryExistsDelete(root + "NewVersion");

        IfFileExistsDelete(root + "NewVersion.zip");


        // Download new version

        Download(latestVersionDownload, root + "NewVersion.zip");


        Extract(root + "NewVersion.zip", root + "NewVersion");

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

string shaderDir = Ansel + "shaders\\msfb_shaders-fc9168ef110b16a8f187a5bf94d2229fda243ad9\\";


//Check if any of the files exists, if it does it gets deleted

// I know this is alot of manual work i don't care to make something dynamically do this
// Like it works, alright

IfFileExistsDelete(Ansel + "shaders.zip");
IfDirectoryExistsDelete(Ansel + "shaders");
IfFileExistsDelete(Ansel + "AgX.fx");
IfFileExistsDelete(Ansel + "AgX-default_contrast.lut.png");
IfFileExistsDelete(Ansel + "dh_rtgi.fx");
IfFileExistsDelete(Ansel + "Limbo_Mod.fx");
IfFileExistsDelete(Ansel + "SnowScape.fx");
IfFileExistsDelete(Ansel + "Deband.fx");
IfFileExistsDelete(Ansel + "DiffuseGlow.fx");
IfFileExistsDelete(Ansel + "FGFXLargeScalePerceptualObscuranceIrradiance.fx");
IfFileExistsDelete(Ansel + "GILT.fx");
IfFileExistsDelete(Ansel + "GILT1.fx");
IfFileExistsDelete(Ansel + "GILT2.fx");
IfFileExistsDelete(Ansel + "GILT3.fx");
IfFileExistsDelete(Ansel + "GILT4.fx");
IfFileExistsDelete(Ansel + "GILT5.fx");
IfFileExistsDelete(Ansel + "GILT4_NoNoise.fx");
IfFileExistsDelete(Ansel + "Glamayre_Fast_Effects.fx");
IfFileExistsDelete(Ansel + "Bumpmapping.fx");
IfFileExistsDelete(Ansel + "Deblur.fx");
IfFileExistsDelete(Ansel + "DeblurUpscaler.fx");
IfFileExistsDelete(Ansel + "HBAO.fx");
IfFileExistsDelete(Ansel + "Low_sats_check.fx");
IfFileExistsDelete(Ansel + "Low_saturation_check.fx");
IfFileExistsDelete(Ansel + "White Point 2D.fx");
IfFileExistsDelete(Ansel + "White Point RGB 2D.fx");
IfFileExistsDelete(Ansel + "White Point_No_Debug.fx");
IfFileExistsDelete(Ansel + "xy Primaries 2D.fx");
IfFileExistsDelete(Ansel + "Y_gamma.fx");
IfFileExistsDelete(Ansel + "Y_Invert.fx");
IfFileExistsDelete(Ansel + "256color.fx");
IfFileExistsDelete(Ansel + "ASSMAA.fx");
IfFileExistsDelete(Ansel + "FSMAA.fx");
IfFileExistsDelete(Ansel + "HQAA.fx");
IfFileExistsDelete(Ansel + "HQAALite.fx");
IfFileExistsDelete(Ansel + "ImageSoften.fx");
IfFileExistsDelete(Ansel + "QXAA.fx");
IfFileExistsDelete(Ansel + "SmartBrightnessBooster.fx");
IfFileExistsDelete(Ansel + "STAA.fx");
IfFileExistsDelete(Ansel + "TSMAA.fx");
IfFileExistsDelete(Ansel + "TSMAA2.fx");
IfFileExistsDelete(Ansel + "XHQAA.fx");
IfFileExistsDelete(Ansel + "MC_SSAO.fx");
IfFileExistsDelete(Ansel + "MC_Tonemap.fx");
IfFileExistsDelete(Ansel + "MC_TonemapHDR.fx");
IfFileExistsDelete(Ansel + "NGLighting_specular.fx");
IfFileExistsDelete(Ansel + "NGLighting-Configs_specular.fxh");
IfFileExistsDelete(Ansel + "NGLighting-Shader_specular.fxh");
IfFileExistsDelete(Ansel + "NGLightingUI_specular.fxh");
IfFileExistsDelete(Ansel + "Volumetric Fog V2.0.fx");
IfFileExistsDelete(Ansel + "Reinhard.fx");
IfFileExistsDelete(Ansel + "5XBR_NoBlend.fx");
IfFileExistsDelete(Ansel + "ShaderFastMathLib.h");
IfFileExistsDelete(Ansel + "YASSGI_bleu.png");
IfFileExistsDelete(Ansel + "YASSGI_old_tracer.fx");
IfFileExistsDelete(Ansel + "ZN_GI.fx");
IfFileExistsDelete(Ansel + "ZNbluenoise512.png");
IfFileExistsDelete(Ansel + "GloomAO.fx");
IfFileExistsDelete(Ansel + "Droste.fx");
IfFileExistsDelete(Ansel + "LongExposure.fx");
IfFileExistsDelete(Ansel + "RealLongExposure.fx");
IfFileExistsDelete(Ansel + "FGFXFastCascadedSeparableBlur16X.fx");
IfFileExistsDelete(Ansel + "PerfectPerspective.fx");
IfFileExistsDelete(Ansel + "Particles.fx");
IfFileExistsDelete(Ansel + "AtmosphericDensity.fx");
IfFileExistsDelete(Ansel + "MShaders.zip");
IfDirectoryExistsDelete(Ansel + "MShaders");
IfDirectoryExistsDelete(Ansel + "Include");
IfFileExistsDelete(Ansel + "PD80_03_Shadows_Midtones_Highlights.fx");
IfFileExistsDelete(Ansel + "PD80_04_Color_Balance.fx");
IfFileExistsDelete(Ansel + "PD80_04_Color_Gradients.fx");
IfFileExistsDelete(Ansel + "PD80_04_Selective_Color.fx");
IfFileExistsDelete(Ansel + "PD80_04_Selective_Color_v2.fx");




// Download shaders

Download("https://github.com/person12306464/msfb_shaders/archive/fc9168ef110b16a8f187a5bf94d2229fda243ad9.zip", Ansel + "shaders.zip");


Console.WriteLine("Installing shaders...");


// Extract
Extract(Ansel + "shaders.zip", Ansel + "shaders");
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
File.Move(shaderDir + "prod80\\PD80_03_Shadows_Midtones_Highlights.fx", Ansel + "PD80_03_Shadows_Midtones_Highlights.fx");
File.Move(shaderDir + "prod80\\PD80_04_Color_Balance.fx", Ansel + "PD80_04_Color_Balance.fx");
File.Move(shaderDir + "prod80\\PD80_04_Color_Gradients.fx", Ansel + "PD80_04_Color_Gradients.fx");
File.Move(shaderDir + "prod80\\PD80_04_Selective_Color.fx", Ansel + "PD80_04_Selective_Color.fx");
File.Move(shaderDir + "prod80\\PD80_04_Selective_Color_v2.fx", Ansel + "PD80_04_Selective_Color_v2.fx");


Directory.Delete(Ansel + "shaders", true);

// That took a while to do...



// Now for GloomAO..

Console.WriteLine("Downloading GloomAO...");

Download("https://raw.githubusercontent.com/BlueSkyDefender/AstrayFX/910e3213a846b34dd65d94e84b61b61fca69dd6d/Shaders/GloomAO.fx", Ansel + "GloomAO.fx");


// Start modifying GloomAO

Console.WriteLine("Patching GloomAO...");


ChangeLine("", Ansel + "GloomAO.fx", 467);

ChangeLine("		zBuffer = rcp(Z.y * C.y + C.x);", Ansel + "GloomAO.fx", 708);
ChangeLine("		zBufferWH = Far * NearWH / (Far + zB.y * (NearWH - Far));", Ansel + "GloomAO.fx", 709);

ChangeLine("		zBuffer = rcp(Z.x * C.y + C.x);", Ansel + "GloomAO.fx", 713);
ChangeLine("		zBufferWH = Far * NearWH / (Far + zB.x * (NearWH - Far));", Ansel + "GloomAO.fx", 714);

ChangeLine("", Ansel + "GloomAO.fx", 1009);
ChangeLine("", Ansel + "GloomAO.fx", 1010);
ChangeLine("", Ansel + "GloomAO.fx", 1011);
ChangeLine("", Ansel + "GloomAO.fx", 1012);
ChangeLine("", Ansel + "GloomAO.fx", 1013);


// Particles

Console.WriteLine("Downloading Particles...");

Download("https://raw.githubusercontent.com/crosire/reshade-shaders/6b452c4a101ccb228c4986560a51c571473c517b/ShadersAndTextures/Particles.fx", Ansel + "Particles.fx");


Console.WriteLine("Patching Particles...");

AddLine(Ansel + "Particles.fx", 180);
AddLine(Ansel + "Particles.fx", 180);
AddLine(Ansel + "Particles.fx", 180);
AddLine(Ansel + "Particles.fx", 180);
AddLine(Ansel + "Particles.fx", 180);
AddLine(Ansel + "Particles.fx", 180);


ChangeLine("float getBUFFER_HEIGHT()", Ansel + "Particles.fx", 182);
ChangeLine("{", Ansel + "Particles.fx", 183);
ChangeLine("	return BUFFER_HEIGHT;", Ansel + "Particles.fx", 184);
ChangeLine("}", Ansel + "Particles.fx", 185);

ChangeLine("	#if (getBUFFER_HEIGHT() <= 720)", Ansel + "Particles.fx", 189);
ChangeLine("	#elif (getBUFFER_HEIGHT() <= 1080)", Ansel + "Particles.fx", 191);
ChangeLine("	#elif (getBUFFER_HEIGHT() <= 1440)", Ansel + "Particles.fx", 193);
ChangeLine("	#elif (getBUFFER_HEIGHT() <= 2160)", Ansel + "Particles.fx", 195);



// Atmospheric Density

Console.WriteLine("Downloading AtmosphericDensity...");

Download("https://github.com/TreyM/MShaders-1/archive/d38b1af92d047b96819c898400919798e265c1cd.zip", Ansel + "MShaders.zip");


Extract(Ansel + "MShaders.zip", Ansel + "MShaders");
File.Delete(Ansel + "MShaders.zip");
File.Move(Ansel + "MShaders\\MShaders-1-d38b1af92d047b96819c898400919798e265c1cd\\Shaders\\MShaders\\AtmosphericDensity.fx", Ansel + "AtmosphericDensity.fx");
Directory.Move(Ansel + "MShaders\\MShaders-1-d38b1af92d047b96819c898400919798e265c1cd\\Shaders\\MShaders\\Include", Ansel + "Include");
Directory.Delete(Ansel + "MShaders", true);



Console.WriteLine("Patching AtmosphericDensity...");


ChangeLine("    #define ENABLE_MISC_CONTROLS 1", Ansel + "AtmosphericDensity.fx", 53);
ChangeLine("    float  depth, sky;", Ansel + "AtmosphericDensity.fx", 486);
ChangeLine("", Ansel + "AtmosphericDensity.fx", 493);
ChangeLine("", Ansel + "AtmosphericDensity.fx", 526);
ChangeLine("", Ansel + "AtmosphericDensity.fx", 527);
ChangeLine("", Ansel + "AtmosphericDensity.fx", 528);
ChangeLine("", Ansel + "AtmosphericDensity.fx", 546);

ChangeLine("UI_COMBO (AUTO_COLOR, \"Fog Color Mode\", \"\", 0, 1,", Ansel + "AtmosphericDensity.fx", 66);



// MXAO IL

Console.WriteLine("Downloading qUINT_mxao_il...");

Download("https://raw.githubusercontent.com/martymcmodding/qUINT/98fed77b26669202027f575a6d8f590426c21ebd/Shaders/qUINT_mxao.fx", Ansel + "qUINT_mxao_il.fx");

ChangeLine(" #define MXAO_ENABLE_IL			1	//[0 or 1]	    Enables Indirect Lighting calculation. Will cause a major fps hit.", Ansel + "qUINT_mxao_il.fx", 31);







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
                  "PD80_03_Shadows_Midtones_Highlights.fx (By prod80, modifed by Person123, UI options are changed to work better in Ansel)\n" +
                  "PD80_04_Color_Balance.fx (By prod80, modifed by Person123, UI options are changed to work better in Ansel)\n" +
                  "PD80_04_Color_Gradients.fx (By prod80, modifed by Person123, UI options are changed to work better in Ansel)\n" +
                  "PD80_04_Selective_Color.fx (By prod80, modifed by Person123, UI options are changed to work better in Ansel)\n" +
                  "PD80_04_Selective_Color_v2.fx (By prod80, modifed by Person123, UI options are changed to work better in Ansel)\n" +
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
