import React from "react";
import Head from "next/head";

export default function Layout({ children }) {
    return (
        <>
            <Head>
                <title>Tickd Technical</title>
            </Head>
            <main className='container'>
                {children}
            </main>
        </>
    )
}