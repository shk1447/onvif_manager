import axios from 'axios';

// http 통신 모듈(향후 세션 체크 및 통신에 대한 관리를 위해 모듈로 이용)
export const Http = {
  post: async (
    url: string,
    body: any,
    config: any = {},
    query: any = undefined,
  ) => {
    if (query) {
      const query_string = new URLSearchParams(
        query as Record<string, string>,
      ).toString();

      if (query_string) url += '?' + query_string;
    }
    return axios
      .post(url, body, config)
      .then(function (response) {
        return response.data;
      })
      .catch(function (err) {
        throw err.response.data;
      });
  },
  get: async (url: string, config: any = {}, query: any = undefined) => {
    if (query) {
      const query_string = new URLSearchParams(
        query as Record<string, string>,
      ).toString();

      if (query_string) url += '?' + query_string;
    }
    config['headers'] = [{ key: 'Cache-Control', value: 'no-cache' }];
    return axios
      .get(url, config)
      .then(function (response) {
        return response.data;
      })
      .catch(function (err) {
        throw err.response.data;
      });
  },
  put: async (
    url: string,
    body: any,
    config: any = {},
    query: any = undefined,
  ) => {
    if (query) {
      const query_string = new URLSearchParams(
        query as Record<string, string>,
      ).toString();

      if (query_string) url += '?' + query_string;
    }
    return axios
      .put(url, body, config)
      .then(function (response) {
        return response.data;
      })
      .catch(function (err) {
        throw err.response.data;
      });
  },
  delete: async (url: string, config: any = {}, query: any = {}) => {
    if (query) {
      const query_string = new URLSearchParams(
        query as Record<string, string>,
      ).toString();

      if (query_string) url += '?' + query_string;
    }
    return axios
      .delete(url, config)
      .then(function (response) {
        return response.data;
      })
      .catch(function (err) {
        throw err.response.data;
      });
  },
};
