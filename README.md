<h1 align="center"> 📱 BFF para uma App Mobile 📱</h1>

<p align="center">
 <a href="#status">Status</a> • 
 <a href="#objective">Objetivo</a> •
 <a href="#installation">Desenvolvimento</a> • 
 <a href="#technology">Tecnologias</a> • 
 <a href="#author">Autor</a> • 
 <a href="#licence">Licença</a>
</p>

<h2 align="center" id=status> 
	⌛ Concluído ⌛
</h2>

<h2 id=objective>📜 Sobre</h2>
Criacao de uma aplicacao BFF API simples no modelo Restfull em .Net. <br>
O modelo de dados esta simples porque o foco esta em abstrair alguns conhecimentos.

<h2 id=installation>✔️ Instalacao</h2>

Você precisa ter o Visual Studio 2022 ou VsCode instalado para testar o projeto.</br>
O projeto roda sob o SDK .Net 8.

Clone o projeto principal desta URL:

~~~
https://github.com/danhpaiva/todo-api-mvc-net
~~~

Execute o projeto principal.

Agora, Rode o BFF e acesse a URL:

~~~
https://localhost:7089/swagger/index.html
~~~

Clique no metodo Get, e aguarde retornar todas as tarefas:

~~~
https://localhost:7089/api/bff/todos
~~~

Voce recebera um JSON semelhante a este:

~~~
[
  {
    "id": 1,
    "title": "XUnit",
    "isDone": true
  }
]
~~~


<h2 id=technology>🧰 Tecnologias</h2>

As seguintes tecnologias foram utilizadas neste projeto:

- IDE: <a href="https://visualstudio.microsoft.com/pt-br/vs/">Visual Studio 2022</a>
- SDK: <a href="https://dotnet.microsoft.com/pt-br/download/dotnet/8.0">.Net 8</a>
- xUnit: <a href="https://github.com/xunit/xunit">xUnit para Testes</a>
  
<h2 id=author>😎 Autor</h2>

Developed by <a href="https://www.linkedin.com/in/danhpaiva/" target="_blank">Daniel Paiva</a>

<h2 id=licence>🆓 Licença</h2>

Este projeto está sob a licença
<a href="https://github.com/danhpaiva/todo-mobile-bff/blob/main/LICENSE" target="_blank">MIT</a>