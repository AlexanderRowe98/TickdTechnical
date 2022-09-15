import { useState, useRef } from "react";
import Loader from "./_loader";

export default function CsvForm(props) {
    const [selectedFile, setSelectedFile] = useState(null);
    const fileInputElement = useRef(null);
    const [fileName, setFileName] = useState("Choose a file");
    const [loader, setLoader] = useState(null);

    const handleChange = (e) => {
        setSelectedFile(e.target.files[0]);
        setFileName(e.target.files[0].name);
    }

    const handleSubmit = (event) => {
        event.preventDefault();
        if (selectedFile) {            
            submitFile();
            event.target.files = null;
        }
        else {
            alert('Please add a file before submitting');
        }
    }

    const submitFile = () => {
        console.log('submit func');
        if (selectedFile.type != "text/csv") {
            alert('Please ensure that the file you upload is in the format `.csv`');
            setFileName("Choose a file");
            fileInputElement.current.value = null;
            props.response(null);
        }
        else {
            console.log('its csv');
            setLoader(<Loader />);
            var data = new FormData()
            data.append('file', selectedFile)
            fetch('/api/meter-reading-uploads', {
                method: 'POST',
                body: data
            }).then((response) => response.json()
            ).then((data) => {
                handleData(data)
            }).catch(
                error => console.error(error)
            );

            setSelectedFile(null);
            fileInputElement.current.value = null;
        }
    }

    const handleData = (data) => {
        props.response(data);
        setLoader(null);
        setFileName("Choose another file");
    }

    return (
        <>
            {loader}
            <form onSubmit={handleSubmit}>
                <h2>Upload Meter Readings</h2>
                <label htmlFor="file-upload" className="file-upload">
                    {fileName}
                </label>
                <input
                    id="file-upload"
                    className="file-upload"
                    ref={fileInputElement}
                    type="file"
                    onChange={(e) => handleChange(e)}                    
                />
                <input type="submit" />
            </form>
        </>
    )
}