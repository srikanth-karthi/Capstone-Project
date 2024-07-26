
export function toggleDisplay(selectorType, selector, shouldHide) {
    let elements;
  
    if (selectorType === 'id') {
      elements = [document.getElementById(selector)];
    } else if (selectorType === 'class') {
      elements = document.getElementsByClassName(selector);
    } else {
      console.error('Invalid selector type. Use "id" or "class".');
      return;
    }
  
  
    for (let element of elements) {
      if (element) {
        element.style.display = shouldHide;
      } else {
        console.error(`Element with ${selectorType} "${selector}" not found.`);
      }
    }
  }
  