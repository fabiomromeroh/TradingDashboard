const baseURL = 'https://localhost:7186/api'

const handleResponse = async (response: Response) => {
  if (!response.ok) {
    throw new Error(`API Error: ${response.status} ${response.statusText}`);
  }
  return response.json();
}

const api = {
  get: (url: string, config?: any) => fetch(`${baseURL}${url}`, { ...config, method: 'GET' }).then(handleResponse),
  post: (url: string, data?: any, config?: any) => fetch(`${baseURL}${url}`, { ...config, method: 'POST', body: JSON.stringify(data), headers: { 'Content-Type': 'application/json', ...config?.headers } }).then(handleResponse),
  put: (url: string, data?: any, config?: any) => fetch(`${baseURL}${url}`, { ...config, method: 'PUT', body: JSON.stringify(data), headers: { 'Content-Type': 'application/json', ...config?.headers } }).then(handleResponse),
  delete: (url: string, config?: any) => fetch(`${baseURL}${url}`, { ...config, method: 'DELETE' }).then(handleResponse),
}

export default api
