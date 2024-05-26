import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import { Header } from "@/components/ui/header";
import QueryContext from "@/contexts/query-context";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "Farmácias PagueMais",
  description: "Projeto em C# + Next.js para A3 do Prof. Padilha",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <QueryContext>
      <html lang="pt-br">
        <body className={inter.className}>
          <Header />

          <main className="px-96 py-12">{children}</main>
        </body>
      </html>
    </QueryContext>
  );
}
