export function arrayToCommaSeperatedLString(collection, appendAndBeforePenultimateElement) {
    let label = "";

    if (collection == undefined || collection == null) {
        label = "error";
    }
    else {
        if (collection.length == 1) {

            label = collection[0];

        } else if (collection.length == 2) {

            label = collection[0] + " and " + collection[1];

        } else {

            if (collection.length > 2) {

                let penultimateIndex = collection.length - 2;
                let lastIndex = collection.length - 1;

                collection.map((item, index) => {

                    if (index < penultimateIndex) {
                        label = label + user.name + ", ";
                    }
                    else if (index == penultimateIndex) {
                        if (appendAndBeforePenultimateElement == true) {
                            label = label + user.name + " and ";
                        }
                        else {
                            label = label + user.name + ", ";
                        }
                    }
                    else if (index == lastIndex) {
                        label = label + user.name;
                    }

                });

            }
        }
    }

    return label;
}