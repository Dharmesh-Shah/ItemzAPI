window.initializeResizable = () => {
    console.log("initializeResizable function called"); // Debugging log

    interact('.resizable')
        .resizable({
            edges: { left: false, right: true, bottom: false, top: false }, // Enable resizing from the right edge
            listeners: {
                move(event) {
                    console.log("Resizing in progress"); // Debugging log
                    let { x, y } = event.target.dataset;
                    x = (parseFloat(x) || 0) + event.deltaRect.left;

                    Object.assign(event.target.style, {
                        width: `${event.rect.width}px`,
                        transform: `translate(${x}px, 0px)`
                    });

                    Object.assign(event.target.dataset, { x, y });
                },
                end(event) {
                    console.log("Resize ended"); // Debugging log
                    // Ensure the width is set after resizing ends
                    let width = event.rect.width;
                    event.target.style.width = `${width}px`;
                    event.target.dataset.width = width; // Persist the width as a dataset attribute
                }
            },
            modifiers: [
                interact.modifiers.restrictEdges({
                    outer: 'parent',
                    endOnly: true,
                }),
                interact.modifiers.restrictSize({
                    min: { width: 300 },
                    max: { width: window.innerWidth - 50 } // Allow maximum width up to the window width minus some padding
                }),
            ],
            inertia: true
        });
}

window.addEventListener('DOMContentLoaded', () => {
    console.log("DOMContentLoaded event triggered"); // Debugging log
    const maxWidth = window.innerWidth * 0.27;
    interact('.resizable')
        .resizable({
            modifiers: [
                interact.modifiers.restrictSize({
                    min: { width: 300 },
                    max: { width: maxWidth }
                }),
            ]
        });
});

// Recalculate on window resize
window.addEventListener('resize', () => {
    console.log("Resize event triggered"); // Debugging log
    const maxWidth = window.innerWidth * 0.27;
    interact('.resizable')
        .resizable({
            modifiers: [
                interact.modifiers.restrictSize({
                    min: { width: 300 },
                    max: { width: maxWidth }
                }),
            ]
        });
});


// Visualize selected TreeView Node via automatic scrolling.

function scrollToElementById(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }
}

// Open URL In New Tab 

function openInNewTab(url) {
    window.open(url, '_blank');
}

