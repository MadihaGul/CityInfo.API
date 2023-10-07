import { useState } from "react";
import { City } from "../types/city";
import config from "../config";

const CityList = () =>
{
    const [cities, setCities] = useState<City[]>([]);
    const fetchCities = async () => {
        const rsp = await fetch(`${config.baseApiUrl}/api/cities`)
        const cities = await rsp.json();
        setCities(cities);
    }
    fetchCities();

    return (
        <div>
          <div className="row mb-2">
            <h5 className="themeFontColor text-center">
              Cities
            </h5>
          </div>
          <table className="table table-hover">
            <thead>
              <tr>
                <th>Name</th>
                <th>Description</th>
              </tr>
            </thead>
            <tbody>
              {cities.map((c: City) => (
                  <tr key={c.id}>
                    <td>{c.name}</td>
                    <td>{c.description}</td>
                    <td>{c.pointsOfInterest}</td>
                  </tr>
                ))}
            </tbody>
          </table>
          
        </div>
      );
}

export default CityList;