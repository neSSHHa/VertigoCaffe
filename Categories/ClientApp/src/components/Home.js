import React, { useState, useEffect } from 'react';

const Home = () => {
    const [cards, setCards] = useState([]);
    const [selectedCard, setSelectedCard] = useState(null);
    const [previousCards, setPreviousCards] = useState([]);

    useEffect(() => {
        // Fetch the main categories
        fetch('http://localhost:5050/customer/home/proizvodi')
            .then(response => response.json())
            .then(data => {
                setCards(data);
                setPreviousCards([]);
            })
            .catch(error => console.log(error));
    }, []);

    const handleItemClick = (item) => {
        // Display the details of the clicked item
        setPreviousCards([...previousCards, cards]);
        setSelectedCard(item);
    };

    const handleCategoryClick = (category) => {
        // Fetch the subcategories or items of the clicked category
        fetch(`http://localhost:5050/customer/home/proizvodi/${category.id}`)
            .then(response => response.json())
            .then(data => {
                setPreviousCards([...previousCards, cards]);
                setCards(data);
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
            <div>
                {selectedCard ? (
                    <div>
                        <h2>{selectedCard.name}</h2>
                        <p>{selectedCard.description}</p>
                        <img src={selectedCard.image} alt={selectedCard.name} />
                        <p>{selectedCard.price}</p>
                        {previousCards.length > 0 && (
                            <button onClick={handleBackButtonClick}>Go Back</button>
                        )}
                    </div>
                ) : (
                    <ul>
                        {cards.map(card => (
                            <li key={card.id} onClick={() => {
                                if (card.type.name === 'Proizvod') {
                                    handleItemClick(card);
                                } else {
                                    handleCategoryClick(card);
                                }
                            }}>
                                {card.name}
                            </li>
                        ))}
                    </ul>
                )}
                {previousCards.length > 0 && !selectedCard && (
                    <button onClick={handleBackButtonClick}>Go Back</button>
                )}
            </div>
        </div>
    );
};

export default Home;
