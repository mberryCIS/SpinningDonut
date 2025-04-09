using System;
using System.Text;
using System.Threading;

class Donut
{
    static void Main()
    {
        int width = 80;
        int height = 24;
        double A = 0, B = 0;
        double thetaSpacing = 0.07, phiSpacing = 0.02;
        double R1 = 1;
        double R2 = 2;
        double K2 = 5;
        double K1 = width * K2 * 3 / (8 * (R1 + R2));

        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            double[] zBuffer = new double[width * height];
            char[] output = new char[width * height];
            Array.Fill(output, ' ');
            Array.Fill(zBuffer, 0);

            for (double theta = 0; theta < 2 * Math.PI; theta += thetaSpacing)
            {
                for (double phi = 0; phi < 2 * Math.PI; phi += phiSpacing)
                {
                    double sinTheta = Math.Sin(theta);
                    double cosTheta = Math.Cos(theta);
                    double sinPhi = Math.Sin(phi);
                    double cosPhi = Math.Cos(phi);

                    double circleX = R2 + R1 * cosTheta;
                    double circleY = R1 * sinTheta;

                    double x = circleX * (Math.Cos(B) * cosPhi + Math.Sin(A) * Math.Sin(B) * sinPhi) - circleY * Math.Cos(A) * Math.Sin(B);
                    double y = circleX * (Math.Sin(B) * cosPhi - Math.Sin(A) * Math.Cos(B) * sinPhi) + circleY * Math.Cos(A) * Math.Cos(B);
                    double z = K2 + Math.Cos(A) * circleX * sinPhi + circleY * Math.Sin(A);
                    double ooz = 1 / z;

                    int xp = (int)(width / 2 + K1 * ooz * x);
                    int yp = (int)(height / 2 - K1 * ooz * y);

                    int idx = xp + yp * width;
                    if (idx >= 0 && idx < width * height)
                    {
                        if (ooz > zBuffer[idx])
                        {
                            zBuffer[idx] = ooz;
                            int luminanceIndex = (int)(8 * ((Math.Cos(phi) * cosTheta * Math.Sin(B) - Math.Cos(A) * cosTheta * sinPhi - Math.Sin(A) * sinTheta + Math.Cos(B) * (Math.Cos(A) * sinTheta - cosTheta * sinPhi * Math.Sin(A)))));
                            char[] luminance = { '.', ',', '-', '~', ':', ';', '=', '!', '*', '#', '$', '@' };
                            output[idx] = luminance[Math.Max(0, Math.Min(luminance.Length - 1, luminanceIndex))];
                        }
                    }
                }
            }

            Console.SetCursorPosition(0, 0);
            for (int k = 0; k < width * height; k++)
            {
                Console.Write(output[k]);
                if ((k + 1) % width == 0)
                    Console.Write('\n');
            }

            A += 0.04;
            B += 0.02;

            Thread.Sleep(30);
        }
    }
}
