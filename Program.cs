using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using System.Globalization;

namespace ConversorDeMoeda
{
    class MyClass
    { 
        private static readonly HttpClient client = new HttpClient(); 
        public async Task Teste()
        {
            client.BaseAddress = new Uri("https://economia.awesomeapi.com.br/last/USD-BRL,EUR-BRL,BTC-BRL");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpResponseMessage response = await client.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    
                    JsonNode jsonObject = JsonNode.Parse(jsonResponse);
                    //Console.WriteLine(jsonObject.ToString());

                    
                    string resposta = "sim";
                    while (resposta.ToLower() == "sim")
                    {
                        Console.Write("Qual valor em real você deseja converter: ");
                        float valorReal = float.Parse(Console.ReadLine());

                        Console.Write("Agora, digite para qual moeda deseja converter: (dolar, euro)");
                        string tipoMoeda = Console.ReadLine().ToLower();

                        float valorMultiplicado = 0;
                        string code = "";
                        string valor = "";

                        if (tipoMoeda == "dolar")
                        {
                            JsonNode usdBrl = jsonObject["USDBRL"];
                            code = (string)usdBrl["code"];
                            valor = (string)usdBrl["high"];
                            valorMultiplicado = valorReal * float.Parse(valor, CultureInfo.InvariantCulture);
                            
                        }else if (tipoMoeda == "euro")
                        {
                            JsonNode euroBrl = jsonObject["EURBRL"];
                            code = (string)euroBrl["code"];
                            valor = (string)euroBrl["high"];
                            valorMultiplicado = valorReal * float.Parse(valor, CultureInfo.InvariantCulture);
                            
                        }

                        if (valorMultiplicado > 0)
                        {
                            Console.WriteLine($"Valor da cotação do {code} hoje {valor}");
                            Console.WriteLine($"Valor de {valorReal} {code} em REAIS é : R${valorMultiplicado}");
                        }

                        Console.Write("voce deseja converter mais alguma moeda? (sim/nao)");
                        resposta = Console.ReadLine().ToLower();
                    }
                }
                else
                {
                    Console.WriteLine($"Erro na requisição: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ocorreu um erro: {e.Message}");
            }
        }

        static async Task Main(string[] args)
        {
            MyClass conversor = new MyClass();
            await conversor.Teste();
        }
    } 
}