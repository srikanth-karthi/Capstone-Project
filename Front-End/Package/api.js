export const baseUrl = "http://localhost:5232/";

export async function fetchData(
    url,
    httpMethod = "GET",
    body = null,
    isFileUpload = false,isauth=false ) {
    const headers = {
      Authorization: `Bearer ${localStorage.getItem("authToken")}`,
      
    };
    if (!isFileUpload) {
      headers["Content-Type"] = "application/json";
    }
  
    const response = await fetch(`${baseUrl}${url}`, {
      method: httpMethod,
      headers: headers,
      body: isFileUpload ? body : body ? JSON.stringify(body) : undefined,
    });

    if (response.status == 401 && !isauth || response.status == 403 && !isauth) {
      localStorage.clear()
      window.location.href = "/Auth/login.html?authid=3";
      return;
    }
    if (!response.ok) {
      const errorBody = await response.json();
      const errorMessage = `Error ${response.status}: ${response.statusText}`;
      const errorDetails = { status: response.status, statusText: response.statusText, body: errorBody };
      throw new Error(JSON.stringify(errorDetails));
    }

  
    const data = await response.json();
    return data;
  }