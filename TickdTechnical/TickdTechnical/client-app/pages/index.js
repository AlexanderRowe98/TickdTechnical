import { useState } from 'react';
import Layout from '../components/_layout';
import CsvForm from '../components/_form'
import Results from '../components/_results';

export default function Home() {
    const [isResults, setIsResults] = useState(false);
    const [successfulEntries, setSuccessfulEntries] = useState(null);
    const [failedEntries, setfailedEntries] = useState(null);

    const displayResponse = (data) => {
        if (data) {
            setSuccessfulEntries(data.successfulEntries);
            setfailedEntries(data.failedEntries);
            setIsResults(true);
        }
        else {
            setIsResults(false);
        }
    }

    return (
        <Layout>
            <h1>Tickd Technical</h1>
            <CsvForm response={displayResponse} />
            {isResults &&
                <Results success={successfulEntries} failed={failedEntries}/>
            }
        </Layout>
    )
}
