CREATE TABLE `categories` (
  `id` int PRIMARY KEY,
  `name` varchar(255)
);

CREATE TABLE `products` (
  `id` int PRIMARY KEY,
  `description` varchar(255),
  `id_category` int,
  `expiration_days` int,
  `price_unit` float,
  `st_tax` float,
  `price_st` float
);

CREATE TABLE `product_qtty` (
  `id` int,
  `quantity` int,
  `price` float,
  PRIMARY KEY (`id`, `quantity`)
);

CREATE TABLE `paletization` (
  `id` int PRIMARY KEY,
  `qt_box` int,
  `qt_box_layer` int,
  `qt_layer_pallet` int
);

ALTER TABLE `products` ADD FOREIGN KEY (`id_category`) REFERENCES `categories` (`id`);

ALTER TABLE `products` ADD FOREIGN KEY (`id`) REFERENCES `product_qtty` (`id`);

ALTER TABLE `products` ADD FOREIGN KEY (`id`) REFERENCES `paletization` (`id`);
