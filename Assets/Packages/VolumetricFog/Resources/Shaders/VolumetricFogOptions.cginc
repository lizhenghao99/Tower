// Uncomment to render in orthographic mode
//#define FOG_ORTHO

// Uncomment to render the fog alone
//#define FOG_DEBUG

// Uncomment to enable screen-space mask based on mesh volumes
//#define FOG_MASK

// Uncomment to enable screen-space mask based on mesh volumes
//#define FOG_INVERTED_MASK

// Uncomment to compute fog void along the raymarching loop (makes it work when viewing it from the fog)
//#define FOG_VOID_HEAVY_LOOP

// Uncomment to enable smooth scattering rays
//#define FOG_SMOOTH_SCATTERING

// Uncomment to enable Unity directional shadowmap. If commented out, it will use a custom shadow pass.
#define FOG_UNITY_DIR_SHADOWS

// Uncomment to enable directional light cookie
//#define FOG_DIR_SHADOWS_COOKIE

// Comment out to disable Sun light diffusion over fog (improves performance)
#define FOG_DIFFUSION

#define FOG_MAX_POINT_LIGHTS 6

//#define FOG_OF_WAR_HEAVY_LOOP
