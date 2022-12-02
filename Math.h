#pragma once

#include <math.h>

float p_0 = 1013.25;
float R = 8.314;
float T_w = 100;
float latent_heat = 2960;

float calc_ethanol(float T)
{
    return 1.09886224 * T - 0.0417772386 * T * T + 0.0183987596 * T * T * T - 0.000869428333 * T * T * T * T - 0.000168116683 * T * T * T * T * T + 0.0000218878317 * T * T * T * T * T * T - 0.000000677617728 * T * T * T * T * T * T * T;
}

float boiling_point(float p)
{
    return 1/(1/T_w - (R * log(p/p_0))/latent_heat);
}

/*double latent_heat_of_vap_of_water(double p)
{
    double H = 1013,25;
    return H;
}*/


/*double real_t_ethanol(double T, double p)
{
    return 1/(1/T + R*log(p/p_0)/latent_heat_of_vap_of_ethanol(p));
}

double calc_del_t(double T, double p)
{
    return T_w - real_t_ethanol(T, p);
}*/
