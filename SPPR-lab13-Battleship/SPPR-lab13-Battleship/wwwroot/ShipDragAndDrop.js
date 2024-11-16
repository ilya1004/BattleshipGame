class DragAndDrop {
    selectors = {
        dragShip: '[data-js-dnd]',
        placedShip: '[data-placed-ship]',
        cell: '[data-js-cell]'
    }

    stateClasses = {
        isDradding: 'is-dragging',
    }

    initialState = {
        offsetX: null,
        offsetY: null,
        isDragging: false,
        isOnBoard: false,
        currentDraggingElement: null,
    }

    constructor() {
        this.state = { ...this.initialState };
        this.hoveredCell = null;
        this.bindEvents();
    }

    resetState() {
        this.state = { ...this.initialState };
    }

    onPointerDown(event) {
        
        const { target, x, y } = event;
        const isDraggable = target.matches(this.selectors.dragShip);
        const isPlacedShip = target.matches(this.selectors.placedShip);

        if (!isDraggable && !isPlacedShip) {
            return;
        }

        if (isPlacedShip) {
            if (event.button == 2) {

                this.state = {
                    offsetX: null,
                    offsetY: null,
                    isDragging: false,
                    isOnBoard: false,
                    currentDraggingElement: target,
                };

                const draggingElement = this.state.currentDraggingElement;

                draggingElement.remove();

                //this.hoveredCell.appendChild(draggingElement);

                const place = document.getElementById(`port-${draggingElement.id}`);

                this.state.currentDraggingElement.style.left = "";
                this.state.currentDraggingElement.style.top = "";

                this.state.currentDraggingElement.setAttribute('data-js-dnd', '');
                this.state.currentDraggingElement.removeAttribute('data-placed-ship');
                this.state.currentDraggingElement.classList.remove("ship-on-board");

                place.style.position = "relative";

                place.appendChild(draggingElement);

                this.state.currentDraggingElement = null;
            }
        }
        else {
            target.classList.add(this.stateClasses.isDradding);

            const { left, top } = target.getBoundingClientRect();

            this.state = {
                offsetX: x - left,
                offsetY: y - top,
                isDragging: true,
                isOnBoard: false,
                currentDraggingElement: target,
            };

            //const x = event.pageX - this.state.offsetX;
            //const y = event.pageY - this.state.offsetY;

            //this.state.currentDraggingElement.style.left = `${x}px`;
            //this.state.currentDraggingElement.style.top = `${y}px`;

            const place = document.getElementById(`port-${target.id}`);

            place.style.position = "static";
        }
    }

    checkCellsHover(event) {
        message = document.getElementById("message");
        if (!message) {
            return;
        }
        this.cells = document.querySelectorAll('.cell-base');
        this.hoveredCell = null;

        this.cells.forEach(cell => {
            const rect = cell.getBoundingClientRect();
            const x = event.clientX;
            const y = event.clientY;

            // Проверяем, находится ли курсор над клеткой
            if (x >= rect.left && x <= rect.right && y >= rect.top && y <= rect.bottom) {
                this.hoveredCell = cell;
            }
        });

        //console.log(this.hoveredCell);

        // Обновляем сообщение
        if (this.hoveredCell) {
            let x = this.hoveredCell.getAttribute('data-x');
            let y = this.hoveredCell.getAttribute('data-y');
            message.textContent = `Курсор над клеткой ${x}/${y}`;
            //this.hoveredCell.classList.add('ship-hovered-cell'); // Подсветка клетки
        } else {
            message.textContent = "Курсор вне клеток.";
            this.cells.forEach(cell => {
                cell.classList.remove('ship-hovered-cell');
            });
        }

        //this.cells.forEach(cell => {
        //    if (cell !== this.hoveredCell) {
        //        cell.classList.remove('ship-hovered-cell');
        //    }
        //});
    }

    onPointerMove(event) {

        this.checkCellsHover(event);

        if (!this.state.isDragging) {
            return;
        }

        if (!this.state.isOnBoard) {
            const x = event.pageX - this.state.offsetX;
            const y = event.pageY - this.state.offsetY;

            this.state.currentDraggingElement.style.left = `${x}px`;
            this.state.currentDraggingElement.style.top = `${y}px`;

            if (this.hoveredCell) {
                
                this.hoveredCell.classList.add('ship-hovered-cell');

                // Убираем подсветку с других клеток
                this.cells.forEach(cell => {
                    if (cell !== this.hoveredCell) {
                        cell.classList.remove('ship-hovered-cell');
                    }
                });
            }
        }
        else {

        }
    }

    onPointerUp(event) {
        console.log("p-up")
        if (!this.state.isDragging) {
            return;
        }
        //if (this.rightClicked) {
        //    this.rightClicked = false;
        //    return;
        //}

        if (this.hoveredCell && !this.state.isOnBoard) {

            const draggingElement = this.state.currentDraggingElement;

            draggingElement.remove();

            this.hoveredCell.appendChild(draggingElement);

            this.state.currentDraggingElement.style.left = "";
            this.state.currentDraggingElement.style.top = "";

            this.state.currentDraggingElement.removeAttribute('data-js-dnd');
            this.state.currentDraggingElement.setAttribute('data-placed-ship', '');
            this.state.currentDraggingElement.classList.add("ship-on-board");
            //this.state.isOnBoard = true;

            //const draggingElement = this.state.currentDraggingElement;

            //if (draggingElement && draggingElement.parentElement) {
            //    draggingElement.parentElement.removeChild(draggingElement);
            //}

            this.cells.forEach(cell => {
                cell.classList.remove('ship-hovered-cell');
            });
            
        }

        this.state.currentDraggingElement.classList.remove(this.stateClasses.isDradding);
        this.resetState();
    }

    onPointerDoubleClick(event) {

    }

    onPointerRightClick(event) {
        if (document.getElementById("table-wrapper-board")) {
            event.preventDefault();
        }
    }


    bindEvents() {
        document.addEventListener('pointerdown', (event) => this.onPointerDown(event));
        document.addEventListener('pointermove', (event) => this.onPointerMove(event));
        document.addEventListener('pointerup', (event) => this.onPointerUp(event));
        document.addEventListener('dblclick', (event) => this.onPointerDoubleClick(event));

        //const place = document.getElementById("table-wrapper-board");
        //console.log(place);

        document.addEventListener('contextmenu', (event) => this.onPointerRightClick(event));
        //document.addEventListener('mouseenter', (event) => this.onPointerUp(event));
        //document.addEventListener('mouseenter', (event) => this.onPointerUp(event));
    }
}

//if (document.getElementById("cells-grid")) {
    new DragAndDrop();
//}


