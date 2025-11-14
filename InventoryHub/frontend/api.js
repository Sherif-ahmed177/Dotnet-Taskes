export async function fetchItems() {
    const res = await fetch("http://localhost:5000/api/items");
    return res.json();
}

export async function addItem(item) {
    const res = await fetch("http://localhost:5000/api/items", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(item)
    });
    return res.json();
}
