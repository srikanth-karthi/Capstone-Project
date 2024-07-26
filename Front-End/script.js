
function handleCredentialResponse(response) {
    console.log('Google User:', response);
    api(response);
}

async function api(response) {
    try {
        const data = await fetch('http://localhost:5232/api/google', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                Token: response.credential
            })
        });
        const result = await data.json(); 
        localStorage.setItem("authToken", result.payload.token);
        localStorage.setItem("userId", result.payload.userId);
        if(result.payload.userId==1) 
            {
                window.location.href="../Admin/Admin.html?authid=1" 
            }
            else 
            {
                window.location.href = "/Front-End/User/Dashboard/Dashboard.html?authid=1";
            }
    } catch (e) {
        console.log(e);
    }
}