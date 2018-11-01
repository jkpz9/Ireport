package ireportproducts;

import java.io.Serializable;
import java.math.BigDecimal;

public class Product implements Serializable {
/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
String name;
BigDecimal price;
public Product(String name, BigDecimal price) {
	super();
	this.name = name;
	this.price = price;
}

}
