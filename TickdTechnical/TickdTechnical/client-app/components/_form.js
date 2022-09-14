import { useState } from "react";

export default function CsvForm() {
    const [selectedFile, setSelectedFile] = useState(null);

    const handleSubmit = (event) => {
        event.preventDefault();
        submitFile();
    }

    const submitFile = () => {
        var data = new FormData()
        data.append('file', selectedFile)
        fetch('/api/meter-reading-uploads', {
            method: 'POST',
            body: data
        }).then((response) => response.json())
        .then((data) => {
          console.log('Success:', data);
        }).catch(
            error => console.log(error)
        );
    }

    return (
        <form onSubmit={handleSubmit}>
            <input
                type="file"
                onChange={(e) => setSelectedFile(e.target.files[0])}
            />
            <br />
            <input type="submit" />
        </form>
    )
}