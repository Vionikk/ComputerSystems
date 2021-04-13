#include <iostream>
#include <cmath>
#include <time.h>

double sigmoid(long double x) {
    return 1 / (1 + std::exp(-x));
}
double func(long double x[], long double y) {
    return x[0] * y + x[1] * y + x[2] * y;
}
int main() {
    clock_t tStart = clock();
    long double t_in[3] = { 1, 1, 0 }, t_out = 1, w = -0.37543/*Случайные веса*/, out;
    for (int i = 0; i < 1000000000; i++) {
        out = sigmoid(func(t_in, w));
        w += func(t_in, (t_out - out) * out * (1 - out));
    }
    std::cout << out << std::endl;
    printf("Time taken: %.2fs\n", (double)(clock() - tStart)/CLOCKS_PER_SEC);
}