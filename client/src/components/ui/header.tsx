"use client";

import Link from "next/link";
import {
  NavigationMenu,
  NavigationMenuContent,
  NavigationMenuItem,
  NavigationMenuLink,
  NavigationMenuList,
  NavigationMenuTrigger,
  navigationMenuTriggerStyle,
} from "./navigation-menu";

export const Header = () => {
  return (
    <header className="py-6 px-6 2xl:px-96">
      <NavigationMenu>
        <NavigationMenuList>
          <NavigationMenuItem>
            <NavigationMenuTrigger>Navegação</NavigationMenuTrigger>
            <NavigationMenuContent>
              <NavigationMenuLink
                href="/purchases"
                className={navigationMenuTriggerStyle()}
              >
                Compras
              </NavigationMenuLink>

              <NavigationMenuLink
                href="/clients"
                className={navigationMenuTriggerStyle()}
              >
                Clientes
              </NavigationMenuLink>

              <NavigationMenuLink
                href="/products"
                className={navigationMenuTriggerStyle()}
              >
                Produtos
              </NavigationMenuLink>

              <NavigationMenuLink
                href="/team"
                className={navigationMenuTriggerStyle()}
              >
                Equipe
              </NavigationMenuLink>
            </NavigationMenuContent>
          </NavigationMenuItem>
        </NavigationMenuList>
      </NavigationMenu>
    </header>
  );
};
