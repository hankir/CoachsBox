interface IWebClientEnvironment {
  baseUrl: string;
}

interface IAspPageQuery {
  [key: string]: string;
}

declare let webEnvironment: IWebClientEnvironment;
declare let getAspPageUrl: (page: string, query?: IAspPageQuery) => string;