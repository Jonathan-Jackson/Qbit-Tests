using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace qbit_tests {

    internal class Program {

        public static async Task Main(string[] args) {
            /////
            // Settings...
            /////
            var address = "http://192.168.1.7:8080";
            var userName = "admin";
            var password = "adminadmin";
            /////

            var handler = new HttpClientHandler();
            handler.UseCookies = false;
            var client = new HttpClient(handler);

            string body = string.Empty;
            HttpResponseMessage response = null;

            // 1. Should Fail Auth.
            using (HttpRequestMessage authToFail = new HttpRequestMessage(HttpMethod.Post, $"{address}/api/v2/auth/login")) {
                body = $"username=xxx&password=xx";
                authToFail.Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
                authToFail.Content.Headers.Add("Content-Length", body.Length.ToString());
                authToFail.Headers.Add("Referer", address);

                response = await client.SendAsync(authToFail);
                if (response.Headers.Any(x => string.Equals(x.Key, "Set-Cookie", StringComparison.OrdinalIgnoreCase)))
                    WriteLineColored("WRONG AUTH PASSED!", ConsoleColor.Red);
                else
                    WriteLineColored("CORRECT AUTH PASSED!", ConsoleColor.Green);
            }

            // 2. Should Pass Auth.
            using (var authToPass = new HttpRequestMessage(HttpMethod.Post, $"{address}/api/v2/auth/login")) {
                body = $"username={userName}&password={password}";
                authToPass.Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
                authToPass.Content.Headers.Add("Content-Length", body.Length.ToString());
                authToPass.Headers.Add("Referer", address);

                response = await client.SendAsync(authToPass);
                if (response.IsSuccessStatusCode)
                    WriteLineColored("CORRECT AUTH PASSED!", ConsoleColor.Green);
                else
                    WriteLineColored("WRONG AUTH PASSED!", ConsoleColor.Red);
            }

            string sid = response.Headers.FirstOrDefault(x => string.Equals(x.Key, "Set-Cookie", StringComparison.OrdinalIgnoreCase)).Value.FirstOrDefault();

            if (sid == null) {
                WriteLineColored("Cannot continue due to failed authentication.", ConsoleColor.Red);
                Console.ReadLine();
                Environment.Exit(0);
            }

            // 3. Check we can get torrents.
            Torrent[] torrents = Array.Empty<Torrent>();
            using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"{address}/api/v2/torrents/info")) {
                request.Headers.TryAddWithoutValidation("Cookie", $"{sid}");

                response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode) {
                    torrents = JsonSerializer.Deserialize<Torrent[]>(await response.Content.ReadAsStringAsync());
                    WriteLineColored($"Success getting torrents: {string.Join(", ", torrents.Select(x => x.name))}", ConsoleColor.Green);
                }
                else
                    WriteLineColored("Failed to get torrents.", ConsoleColor.Red);
            }

            if (!torrents.Any()) {
                WriteLineColored("Cannot continue as no torrents were found.", ConsoleColor.Red);
                Console.ReadLine();
                Environment.Exit(0);
            }

            // 4. Pause the torrent.
            string hash = torrents.First().hash;
            using (var request = new HttpRequestMessage(new HttpMethod("GET"), $"{address}/api/v2/torrents/pause?hashes={hash}")) {
                request.Headers.TryAddWithoutValidation("Cookie", $"{sid}");
                request.Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");
                request.Content.Headers.Add("Content-Length", body.Length.ToString());

                response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode) {
                    WriteLineColored($"Success pausing torrent!", ConsoleColor.Green);
                }
                else
                    WriteLineColored("Failed to pause torrent.", ConsoleColor.Red);
            }

            WriteLineColored("Complete!", ConsoleColor.Blue);
            Console.Read();
        }

        private static void WriteLineColored(string value, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ResetColor();
        }
    }
}