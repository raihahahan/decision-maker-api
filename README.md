<!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->
<a name="readme-top"></a>
<!--
*** Thanks for checking out the Best-README-Template. If you have a suggestion
*** that would make this better, please fork the repo and create a pull request
*** or simply open an issue with the tag "enhancement".
*** Don't forget to give the project a star!
*** Thanks again! Now go create something AMAZING! :D
-->



<!-- PROJECT SHIELDS -->
<!--
*** I'm using markdown "reference style" links for readability.
*** Reference links are enclosed in brackets [ ] instead of parentheses ( ).
*** See the bottom of this document for the declaration of the reference variables
*** for contributors-url, forks-url, etc. This is an optional, concise syntax you may use.
*** https://www.markdownguide.org/basic-syntax/#reference-style-links
-->
[![Contributors][contributors-shield]][contributors-url]
[![Forks][forks-shield]][forks-url]
[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![MIT License][license-shield]][license-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<!-- PROJECT LOGO -->
<br />
<div align="center">

<h3 align="center">Decision Maker Web API</h3>

  <p align="center">
    An ASP.NET Web API to help you make decisions.
    <br />
    <a href="https://github.com/raihahahan/decision-maker-api"><strong>Explore the docs »</strong></a>
    <br />
    <br />
    <a href="https://github.com/raihahahan/decision-maker-api">View Demo</a>
    ·
    <a href="https://github.com/raihahahan/decision-maker-api/issues">Report Bug</a>
    ·
    <a href="https://github.com/raihahahan/decision-maker-api/issues">Request Feature</a>
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

[![Product Name Screen Shot][product-screenshot]](https://example.com)

Decision Maker API is a Web API created with ASP.NET. It contains features such as CRUD, authentication, search, pagination, filters and more.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Features and Endpoints
- For a much clearer overview of the schema and endpoints, go to https://localhost:7239/swagger/index.html.

#### Random Decision
- From a list of choices, randomly return a decision.

```
GET /api/RandomDecisionItems
POST /api/RandomDecisionItems
GET /api/RandomDecisionItems/{id}
PUT /api/RandomDecisionItems/{id}
DELETE /api/RandomDecisionItems/{id}
GET /api/RandomDecisionItems/{id}/decide
```

#### Weighted Decision
- From a list of choices, provide a list of criteria affecting your choice.
- For example, for a list of cars, some criteria may be budget, convenience, specs etc.
- Weigh how important each criteria is for each choice.

```
GET /api/WeightedDecisionItems
POST /api/WeightedDecisionItems
GET /api/WeightedDecisionItems/{id}
PUT /api/WeightedDecisionItems/{id}
DELETE /api/WeightedDecisionItems/{id}
POST /api/WeightedDecisionItems/{id}/decide
```

#### Conditional Decision
- From a list of choices, provide a list of conditions affecting your choice.
- For example, for a list of commuting method (e.g. bicycle, public transport, private transport), some conditions may be "Rainy weather", "Must report to work earlier" etc. And for each condition, there are two lists, `include` and `exclude`, each containing a list of choices to include and exclude respectively if the given condition is true.

```
GET /api/ConditionalDecisionItems
POST /api/ConditionalDecisionItems
GET /api/ConditionalDecisionItems/{id}
PUT /api/ConditionalDecisionItems/{id}
DELETE /api/ConditionalDecisionItems/{id}
POST /api/ConditionalDecisionItems/{id}/decide
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->
## Getting Started

### Prerequisites
You will need to install dotnet. See https://dotnet.microsoft.com/en-us/.

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/raihahahan/decision-maker-api.git
   ```
2. Restore the dependencies and tools of the project.
   ```sh
   dotnet restore
   ```
3. Start the project.
   ```sh
   dotnet run
   ```

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- ROADMAP -->
## Roadmap

- [x] CRUD for each feature.
- [x] Search, filters, pagination.
- [ ] Unit testing.
- [ ] Authentication to save the decisions into an account.

See the [open issues](https://github.com/raihahahan/decision-maker-api/issues) for a full list of proposed features (and known issues).

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- CONTACT -->
## Contact
Email: mraihandev@gmail.com
Website: https://mraihan.vercel.app

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

* [Microsoft Learn: Create a Web API](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio)
* [ReadMe template](https://github.com/othneildrew/Best-README-Template)

<p align="right">(<a href="#readme-top">back to top</a>)</p>


<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[contributors-shield]: https://img.shields.io/github/contributors/raihahahan/decision-maker-api.svg?style=for-the-badge
[contributors-url]: https://github.com/raihahahan/decision-maker-api/graphs/contributors
[forks-shield]: https://img.shields.io/github/forks/raihahahan/decision-maker-api.svg?style=for-the-badge
[forks-url]: https://github.com/raihahahan/decision-maker-api/network/members
[stars-shield]: https://img.shields.io/github/stars/raihahahan/decision-maker-api.svg?style=for-the-badge
[stars-url]: https://github.com/raihahahan/decision-maker-api/stargazers
[issues-shield]: https://img.shields.io/github/issues/raihahahan/decision-maker-api.svg?style=for-the-badge
[issues-url]: https://github.com/raihahahan/decision-maker-api/issues
[license-shield]: https://img.shields.io/github/license/raihahahan/decision-maker-api.svg?style=for-the-badge
[license-url]: https://github.com/raihahahan/decision-maker-api/blob/main/LICENSE
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/muhammad-raihan-rizqullah-21b554248
[product-screenshot]: images/screenshot.png
[Next.js]: https://img.shields.io/badge/next.js-000000?style=for-the-badge&logo=nextdotjs&logoColor=white
[Next-url]: https://nextjs.org/
[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[Vue.js]: https://img.shields.io/badge/Vue.js-35495E?style=for-the-badge&logo=vuedotjs&logoColor=4FC08D
[Vue-url]: https://vuejs.org/
[Angular.io]: https://img.shields.io/badge/Angular-DD0031?style=for-the-badge&logo=angular&logoColor=white
[Angular-url]: https://angular.io/
[Svelte.dev]: https://img.shields.io/badge/Svelte-4A4A55?style=for-the-badge&logo=svelte&logoColor=FF3E00
[Svelte-url]: https://svelte.dev/
[Laravel.com]: https://img.shields.io/badge/Laravel-FF2D20?style=for-the-badge&logo=laravel&logoColor=white
[Laravel-url]: https://laravel.com
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[JQuery.com]: https://img.shields.io/badge/jQuery-0769AD?style=for-the-badge&logo=jquery&logoColor=white
[JQuery-url]: https://jquery.com

