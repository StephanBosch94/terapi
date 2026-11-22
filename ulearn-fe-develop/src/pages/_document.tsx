import { Html, Head, Main, NextScript } from 'next/document'


export default function Document() {

    return (
        <Html>
            <Head>
                <style dangerouslySetInnerHTML={{
                    __html: `
                        #initial-loading-screen {
                            position: fixed;
                            top: 0;
                            left: 0;
                            right: 0;
                            bottom: 0;
                            background-color: rgb(255,255,255);
                            display: flex;
                            flex-direction: column;
                            align-items: center;
                            justify-content: center;
                            z-index: 9999;
                        }
                        #initial-loading-screen .spinner {
                            width: 50px;
                            height: 50px;
                            border: 4px solid rgba(114,100,214,0.1);
                            border-top-color: rgb(114,100,214);
                            border-radius: 50%;
                            animation: spin 1s linear infinite;
                        }
                        @keyframes spin {
                            to { transform: rotate(360deg); }
                        }
                        #initial-loading-screen .message {
                            margin-top: 24px;
                            color: rgb(36,41,51);
                            text-align: center;
                            max-width: 400px;
                            padding: 0 20px;
                            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Roboto', 'Oxygen', 'Ubuntu', 'Cantarell', 'Fira Sans', 'Droid Sans', 'Helvetica Neue', sans-serif;
                            font-size: 16px;
                            line-height: 1.5;
                        }
                    `
                }} />
            </Head>
            <body>
                <div id="initial-loading-screen">
                    <div className="spinner"></div>
                    <div className="message">Spinning up VM may take up to 1 minute. Please wait.</div>
                </div>
                <Main />
                <NextScript />
            </body>
        </Html>
    )
}