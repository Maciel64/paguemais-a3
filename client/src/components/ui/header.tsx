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
    <header className="py-6 px-96">
      <NavigationMenu>
        <NavigationMenuList>
          <NavigationMenuItem>
            <NavigationMenuTrigger>Navegação</NavigationMenuTrigger>
            <NavigationMenuContent>
              <Link href="/purchases">
                <NavigationMenuLink className={navigationMenuTriggerStyle()}>
                  Compras
                </NavigationMenuLink>
              </Link>

              <Link href="/clients">
                <NavigationMenuLink className={navigationMenuTriggerStyle()}>
                  Clientes
                </NavigationMenuLink>
              </Link>

              <Link href="/products">
                <NavigationMenuLink className={navigationMenuTriggerStyle()}>
                  Produtos
                </NavigationMenuLink>
              </Link>

              <Link href="/team">
                <NavigationMenuLink className={navigationMenuTriggerStyle()}>
                  Equipe
                </NavigationMenuLink>
              </Link>
            </NavigationMenuContent>
          </NavigationMenuItem>
        </NavigationMenuList>
      </NavigationMenu>
    </header>
  );
};
