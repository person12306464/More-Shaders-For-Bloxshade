# More Shaders For Bloxshade
More Shaders For Bloxshade adds additional shaders for presets to use.

Installer by Person123, everything else by Sync (and the community in general) expect for those shaders I patched.

# YOU NEED [.NET](https://dotnet.microsoft.com/en-us/download) TO RUN THIS


## To compile it yourself:

First make sure you have [.NET](https://dotnet.microsoft.com/en-us/download) installed.

Then right click and open terminal in the project folder and run `dotnet publish -c Release`

You'll find the result in `More Shaders For Bloxshade\bin\Release\net8.0\win-x64\publish`


## Installs the following shaders:


```
AgX.fx (By MrLixm)
dh_rtgi.fx (By AlucardDH)
Limbo_Mod.fx (By BlueSkyDefender, License: https://creativecommons.org/licenses/by/2.0/)
SnowScape.fx (By BlueSkyDefender, License: https://creativecommons.org/licenses/by/2.0/)
Deband.fx (By haasn, modified and optimized for ReShade by JPulowski.)
GILT.fx (By Extravi) (GILT 1-5 are test versions)
Glamayre_Fast_Effects.fx (By Robert Jessop)
Bumpmapping.fx (By guestrr, patched by Person123)
Deblur.fx (By guestrr)
DeblurUpscaler.fx (guestrr)
HBAO.fx (By Ethatron, tomerk and Marty McFly, Ported from ReShade Framework to ReShade 3.0+ by mbah.primbon)
Low_sats_check.fx (By crabshank)
Low_saturation_check.fx (By crabshank)
White Point 2D.fx (By crabshank)
White Point RGB 2D.fx (By crabshank)
White Point_No_Debug.fx (By crabshank)
xy Primaries 2D.fx (By crabshank)
Y_gamma.fx (By crabshank)
Y_Invert.fx (By crabshank)
256color.fx (By lordbean)
ASSMAA.fx (By lordbean)
FSMAA.fx (By lordbean)
HQAA.fx (By lordbean)
HQAALite.fx (By lordbean)
ImageSoften.fx (By lordbean)
QXAA.fx (By lordbean)
SmartBrightnessBooster.fx (By lordbean)
STAA.fx (By lordbean)
TSMAA.fx (By lordbean)
TSMAA2.fx (By lordbean)
XHQAA.fx (By lordbean)
MC_SSAO.fx (By Constantine 'MadCake' Rudenko, License: https://creativecommons.org/licenses/by/4.0/)
MC_Tonemap.fx (By Constantine 'MadCake' Rudenko, License: https://creativecommons.org/licenses/by/4.0/) (Has been modified)
MC_TonemapHDR.fx (By Constantine 'MadCake' Rudenko, License: https://creativecommons.org/licenses/by/4.0/) (Has been modified)
Reinhard.fx (Original code by Marty McFly, Amateur port by Insomnia)
5XBR_NoBlend.fx (By Hyllian, Ported by spiderh @2018)
YASSGI_old_tracer.fx (By Pentalimbed)
ZN_GI.fx (By Zenteon, patched by Sync)
GloomAO.fx (By BlueSkyDefender, License: https://creativecommons.org/licenses/by-nd/4.0, patched by Sync)
Droste.fx (By SirCobra)
LongExposure.fx (By SirCobra)FGFXFastCascadedSeparableBlur16X.fx (By Alex Tuduran, patched by Person123)
PerfectPerspective.fx (By Fubaxiusz, License: http://creativecommons.org/licenses/by-sa/3.0/, patched by Person123) (Has been modified)
Particles.fx (By BlueSkyDefender, License: https://creativecommons.org/licenses/by-nd/4.0/, patched by Person123)
qUINT_mxao_il.fx (By Marty McFly, MXAO with IL enabled.)

Highlights:

NGLighting_specular.fx (By NiceGuy, patched by Extravi, modified by Person123)
Ray traced reflections, for free.

FGFXLargeScalePerceptualObscuranceIrradiance.fx (By Alex Tuduran, patched by Person123)
Adds large scale perceptual obscurance irradiance, ok I'm not exactly sure what this does but it's cool right?

Volumetric Fog V2.0.fx (By NiceGuy, patched by Person123)
Adds some kind of fake volumetric fog by blurring the background and blending it and doing a bunch of other things, it looks really cool actually.

AtmosphericDensity.fx (By TreyM, patched by Sync)
Good fog shader, only "Exact Fog Color" works correctly in Ansel, very broken in general but pair it with Volumetric Fog V2 and you got some good fog.
If this shader worked fully correctly it would've been a replacment for Volumetric Fog V2 but it's more of a replacment for AdaptiveFog in Ansel.

RealLongExposure.fx (By SirCobra)
Long Exposure shader, toggle the UI option on and it will blend the frames together for the defined amount of time.

DiffuseGlow.fx (By Ioxa)
I can't really explain this one, just try it yourself, it's cool.
```
