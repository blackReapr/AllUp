const cartBody = document.querySelector(".cartBody");

const cartPageRemoveProduct = async id => {
    let data
    if (typeof id === "number") {
        const res = await axios.get("/cart/removeproduct/" + id);
        data = res.data;
    }

    removeProductFromCart(data);

    const template = `
                                ${data.map(item => `
                                <tr>
                                        <td>
                                            <div class="product-image">
                                                <img src="/assets/images/product/${item.image}" alt="${item.name}">
                                            </div>
                                        </td>
                                        <td>
                                            <div class="product-title">
                                                <h4 class="title"><a href="/product/detail/${item.id}">${item.name}</a></h4>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="product-price">
                                                <span class="price">€${item.price}</span>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="product-quantity">
                                                <div class="quantity mt-15 d-flex">
                                                    <button type="button" class="sub"><i class="fal fa-minus"></i></button>
                                                    <input type="text" value="1" />
                                                    <button type="button" class="add"><i class="fal fa-plus"></i></button>
                                                </div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="product-total">
                                                <span class="total-amount">€${item.price}</span>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="product-delete">
                                                <a onclick="cartPageRemoveProduct(${item.id})"><i class="fal fa-trash-alt"></i></a>
                                            </div>
                                        </td>
                                    </tr>


                                `).join("")}


    `
    cartBody.innerHTML = template;
}