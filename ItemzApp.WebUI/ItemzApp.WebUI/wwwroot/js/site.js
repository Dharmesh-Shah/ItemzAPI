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

// Which marked function to use based on marked library version

window.markdownToHtml = function (markdown) {
    // Check if 'marked' is a default export
    const markedFunction = typeof marked === 'function' ? marked : marked.parse;
    return markedFunction(markdown);
}


//// For FullScreen for MarkdownEditor
document.addEventListener("DOMContentLoaded", function () {
    function renderMarkdown() {
        const markdownContent = "# Hello World\n\nThis is a **markdown** example.";

        // Use marked.parse to convert markdown to HTML
        const renderedHtml = marked.parse(markdownContent);
        document.getElementById("content").innerHTML = renderedHtml;
    }

    renderMarkdown();

    const applyMarkdownEditorFullscreenStyles = function () {
        const editorContainer = document.querySelector('.CodeMirror-fullscreen');
        const previewPane = document.querySelector('.editor-preview-side.editor-preview-active-side');
        const appBarHeight = document.querySelector('.mud-appbar') ? document.querySelector('.mud-appbar').offsetHeight : 64;
        const navBarWidth = document.querySelector('.mud-nav') ? document.querySelector('.mud-nav').offsetWidth : 250;

        if (editorContainer) {
            editorContainer.style.position = 'fixed';
            editorContainer.style.top = `${appBarHeight}px`;
            editorContainer.style.left = `${navBarWidth}px`;
            editorContainer.style.right = '0';
            editorContainer.style.bottom = '0';
            editorContainer.style.width = `calc(100% - ${navBarWidth}px)`;
            editorContainer.style.height = `calc(100% - ${appBarHeight}px)`;
            editorContainer.style.zIndex = '1000';
            editorContainer.style.backgroundColor = 'white';
            editorContainer.style.padding = '10px';
            editorContainer.style.overflow = 'auto';

            if (previewPane) {
                previewPane.style.position = 'fixed';
                previewPane.style.top = `${appBarHeight}px`;
                previewPane.style.right = '0';
                previewPane.style.bottom = '0';
                previewPane.style.width = '50%';
                previewPane.style.height = `calc(100% - ${appBarHeight}px)`;
                previewPane.style.zIndex = '1000';
                previewPane.style.backgroundColor = 'white';
                previewPane.style.overflow = 'auto';
            }
        } else {
            const editorNormalContainer = document.querySelector('.CodeMirror');
            if (editorNormalContainer) {
                editorNormalContainer.style.position = '';
                editorNormalContainer.style.top = '';
                editorNormalContainer.style.left = '';
                editorNormalContainer.style.right = '';
                editorNormalContainer.style.bottom = '';
                editorNormalContainer.style.width = '';
                editorNormalContainer.style.height = '';
                editorNormalContainer.style.zIndex = '';
                editorNormalContainer.style.backgroundColor = '';
                editorNormalContainer.style.padding = '';
                editorNormalContainer.style.overflow = '';

                if (previewPane) {
                    previewPane.style.position = '';
                    previewPane.style.top = '';
                    previewPane.style.right = '';
                    previewPane.style.bottom = '';
                    previewPane.style.width = '';
                    previewPane.style.height = '';
                    previewPane.style.zIndex = '';
                    previewPane.style.backgroundColor = '';
                    previewPane.style.overflow = '';
                }
            }
        }
    };

    const observer = new MutationObserver(() => {
        applyMarkdownEditorFullscreenStyles();
    });

    const startObserving = function () {
        const editorContainer = document.querySelector('.EasyMDEContainer');
        if (editorContainer) {
            observer.observe(editorContainer, { attributes: true, attributeFilter: ['class'] });
            applyMarkdownEditorFullscreenStyles();
        } else {
            setTimeout(startObserving, 500);
        }
    };

    startObserving();
});
