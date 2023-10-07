import React from 'react';

import './App.css';
import Header from './Header';
import CityList from '../city/CityList';

function App() {
  return (
    <div className="container">
      <Header subtitle="My City App"/>
      <CityList />
    </div>
  );
}

export default App;
