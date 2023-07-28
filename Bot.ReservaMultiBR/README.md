# automate.test
Selenium Web Automation Tool 

A class helper SeleniumHelper.cs tem muitos metodos como: 

- Abrir url
- Obter valor de determinado componente HTML
- Setar valor de determinado componente HTML
- Delay de tempo de renderização, caso a pagina seja lenta
- Verificação se estamos na pagina que queremos, ou seja se estamos na pagina que esperávamos
- Entre OUTROS...
 

1. Exemplo de navegação em paginas 

        static void Main(string[] args)
        {
            Console.WriteLine(".:: 1. Exemplo de Abrir Navegador");
        
            Selenium.GoToUrl("https://www.google.com");

            Console.WriteLine(".:: 2. Exemplo de setar valor no campo");
            Selenium.FillTextBoxByXPath("/html/body/div[1]/div[3]/form/div[1]/div[1]/div[1]/div/div[2]/input", "xjur"); 
           

            Console.WriteLine(".:: 3. Exemplo de cliques");
            Selenium.ClickByXPath("/html/body/div[1]/div[3]/form/div[1]/div[1]/div[4]/center/input[1]"); 
            Selenium.ClickByXPath("/html/body/div[7]/div/div[11]/div/div[2]/div[2]/div/div/div[1]/div/div/div/div/div/div/div[1]/a/h3");


            Console.WriteLine(".:: 4. Exemplo de obter valor");
            var valor = Selenium.GetTextByXPath("/html/body/div[1]/div/div/div/div/article/div/div/section[1]/div[3]/div[1]/div/div[1]/div/div/div[1]/h4");
            Console.WriteLine(".:: 5. Valor obtido: " + valor);

            Selenium.Delay(10000);
            Selenium.Dispose();

            Console.WriteLine(".:: Fim do exemplo");
        }

2. Como copiar o XPath dos componentes em HTML

![COMO_COPIAR_XPATH](https://user-images.githubusercontent.com/21311134/229977753-b6093d38-8b39-49ea-b3e8-7d92247682c3.png)



3. Atualizar versão do chorme

![deixar_chorme_atualizado_nessa_versao](https://user-images.githubusercontent.com/21311134/229977274-9398d263-e849-425f-a6f1-eb09c36ecd2b.PNG)
