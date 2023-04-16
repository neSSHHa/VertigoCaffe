import React, { useState, useEffect } from 'react';
import './itemsandcategories.css';

function Spinner() {
    return (
        <div className="spinner-overlay">
            <div className="spinner"></div>
        </div>
    );
}

const Home = () => {
    const [cards, setCards] = useState([]);
    const [selectedCard, setSelectedCard] = useState(null);
    const [previousCards, setPreviousCards] = useState([]);
    const [loading, setLoading] = useState(true); // add loading state variable

    useEffect(() => {
        setLoading(true); // set loading to true
        // Fetch the main categories
        fetch('https://vertigocaffeadminmenage.azurewebsites.net/customer/home/proizvodi')
            .then(response => response.json())
            .then(data => {
                setCards(data);
                setPreviousCards([]);
                setLoading(false); // set loading to false when data is fetched
            })
            .catch(error => console.log(error));
    }, []);

    const handleItemClick = (item) => {
        // Display the details of the clicked item
        setPreviousCards([...previousCards, cards]);
        setSelectedCard(item);
    };

    const handleCategoryClick = (category) => {
        setLoading(true); // set loading to true
        // Fetch the subcategories or items of the clicked category
        fetch(`https://vertigocaffeadminmenage.azurewebsites.net/customer/home/proizvodi/${category.id}`)
            .then(response => response.json())
            .then(data => {
                setPreviousCards([...previousCards, cards]);
                setCards(data);
                setLoading(false); // set loading to false when data is fetched
            })
            .catch(error => console.log(error));
    };

    const handleBackButtonClick = () => {
        // Set the cards to the previous cards
        const previousCardsCopy = [...previousCards];
        const cardsToSet = previousCardsCopy.pop();
        setCards(cardsToSet);
        setPreviousCards(previousCardsCopy);
        setSelectedCard(null);
    };

    return (
        <div>
            {loading ? <Spinner /> : (
                <div className="card-container">
                    {previousCards.length > 0 && !selectedCard && (
                        <button onClick={handleBackButtonClick}>Back</button>
                    )}
                    {selectedCard ? (
                        <div className="card-details">
                            {previousCards.length > 0 && (
                                <button onClick={handleBackButtonClick}>Back</button>
                            )}
                            <h2>{selectedCard.name}</h2>
                            <div className="card-details-container">
                                <img src={`https://vertigocaffeadminmenage.azurewebsites.net/${selectedCard.image.replace(/\\/g, "/")}`} alt={selectedCard.name} />
                                <div className="card-details-content">
                                    <p>{selectedCard.description}</p>
                                    <div className="card-prices">
                                        <div className="card-price">
                                            <h6>Cena 1:</h6>
                                            <p>{selectedCard.price}</p>
                                        </div>
                                        <div className="card-price">
                                            <h6>Cena 2:</h6>
                                            <p>{selectedCard.price2}</p>
                                        </div>
                                        <div className="card-price">
                                            <h6>Cena 3:</h6>
                                            <p>{selectedCard.price2}</p>
                                        </div>
                                        <div className="card-price">
                                            <h6>Cena 4:</h6>
                                            <p>{selectedCard.price3}</p>
                                        </div>
                                        <div className="card-price">
                                            <h6>Cena 5:</h6>
                                            <p>{selectedCard.price4}</p>
                                        </div>
                                        <div className="card-price">
                                            <h6>Jos cena:</h6>
                                            <p>{selectedCard.morePrices}</p>
                                        </div>
                                    </div>
                                    <p className="kolicina">Za kolicinu: {selectedCard.amount}</p>
                                    <div id="devider"></div>
                                    {selectedCard.appendices && (
                                        <div className="Appendices">
                                            <h2 className="dodatak">Dodaci</h2>
                                            <ul>
                                                {selectedCard.appendices.map(appendence => (
                                                    <li>
                                                        <img src={`https://vertigocaffeadminmenage.azurewebsites.net/${appendence.image.replace(/\\/g, "/")}`} alt={appendence.name} />
                                                        <div className="appendence-list-item-content">
                                                            <h3> {appendence.name}</h3>
                                                            {appendence.price === 0 && (
                                                                <p className="appendence-price">Besplatan Dodatak</p>
                                                            )}
                                                            {appendence.price != 0 && (
                                                                <p className="appendence-price">{appendence.price}</p>
                                                            )}
                                                        </div>
                                                    </li>
                                                ))}
                                            </ul>
                                        </div>
                                    )}
                                </div>
                            </div>
                        </div>

                    ) : (
                        <div className={cards.length > 0 && cards[0].type.name === 'Proizvod' ? "card-list-products" : "card-list"}>
                            <ul id="listofitems">
                                {cards.map(card => (
                                    <li key={card.id} onClick={() => {
                                        if (card.type.name === 'Proizvod') {
                                            handleItemClick(card);
                                        } else {
                                            handleCategoryClick(card);
                                        }
                                    }}>
                                        <div className="card-list-item">
                                            {card.type.name === 'Kategorija' && (
                                                <div className="category-label">Category</div>
                                            )}
                                            <img src={`https://vertigocaffeadminmenage.azurewebsites.net/${card.image.replace(/\\/g, "/")}`} alt={card.name} />
                                            <div className="card-list-item-content">
                                                <h3>{card.name}</h3>
                                                {card.type.name === 'Kategorija' && (
                                                    <p id="description1">{card.description}</p>
                                                )}
                                                {card.type.name === 'Proizvod' && (
                                                    <p id="description2">{card.description}</p>
                                                )}
                                                {card.type.name === 'Proizvod' && (
                                                    <p className="card-price">{card.price}</p>
                                                )}
                                            </div>
                                        </div>
                                    </li>
                                ))}
                            </ul>
                        </div>
                    )}

                </div>
             )}
        </div>

    );
};

export default Home;


