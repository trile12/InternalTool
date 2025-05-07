window.fetchData = async function (url, options) {
    const response = await fetch(url, options);
    const responseText = await response.text();
    return responseText;
}
