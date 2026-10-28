export async function fetchText(url) {
    const response = await fetch(url, { cache: "no-store", headers: { accept: "application/json" } });
    return { status: response.status, body: await response.text() };
}
