export function findItemAboveViewport() {
    const viewport = document.getElementById("log-viewport");
    const top = viewport.getBoundingClientRect().top + viewport.clientTop;
    const items = [...viewport.querySelectorAll("[data-log-id]")];
    const item = items.filter(element => element.getBoundingClientRect().bottom <= top).at(-1);
    return item ? Number(item.dataset.logId) : null;
}