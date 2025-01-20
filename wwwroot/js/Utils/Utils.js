let removeElementFromArray = (array,val) => {
    const index = array.indexOf(val);
    if(index > -1){
        array.splice(index,1);
    }
}

let addIfNotExist = (array,val) => {
    if(!array.includes(val)){
        array.push(val);
    }
    else{
        removeElementFromArray(array,val);
    }
}

export { addIfNotExist };