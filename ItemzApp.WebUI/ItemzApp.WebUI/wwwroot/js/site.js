
// wwwroot/js/site.js
window.initializeResizable = () => {
    const maxWidth = window.innerWidth * 0.36; // Calculation for MaxWidth to be 36% of remaining space.

    interact('.resizable')
        .resizable({
            edges: { left: false, right: true, bottom: false, top: false }, // Enable resizing from left and right edges
            listeners: {
                move(event) {
                    let { x, y } = event.target.dataset;

                    x = (parseFloat(x) || 0) + event.deltaRect.left;

                    Object.assign(event.target.style, {
                        width: `${event.rect.width}px`,
                        transform: `translate(${x}px, 0px)`
                    });

                    Object.assign(event.target.dataset, { x, y });
                },
                end(event) {
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
                    max: { width: maxWidth } // Use the dynamically calculated maximum width
                }),
            ],
            inertia: true
        });
}

window.addEventListener('DOMContentLoaded', () => {
    const maxWidth = window.innerWidth * 0.36;
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
    const maxWidth = window.innerWidth * 0.36;
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
