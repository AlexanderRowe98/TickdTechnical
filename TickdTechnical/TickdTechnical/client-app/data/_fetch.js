export default async function ApiCall(data, handleData, handleError) { 
    fetch('/api/meter-reading-uploads', {
        method: 'POST',
        body: data
    }).then((response) => {
        if (!response.ok) {
            return response.text().then(text => { handleError(text) })
        }
        else {
            return response.json()
        }
    }).then((data) => {
        handleData(data)
    }).catch(
        error => console.error(error)
    );
}