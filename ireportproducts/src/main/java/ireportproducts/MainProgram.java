package ireportproducts;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.OutputStream;
import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import net.sf.jasperreports.engine.JRException;
import net.sf.jasperreports.engine.JasperCompileManager;
import net.sf.jasperreports.engine.JasperExportManager;
import net.sf.jasperreports.engine.JasperFillManager;
import net.sf.jasperreports.engine.JasperPrint;
import net.sf.jasperreports.engine.JasperReport;
import net.sf.jasperreports.engine.data.JRBeanCollectionDataSource;



public class MainProgram {
 public static void main(String[] args) {
	String userHomeDir = System.getProperty("user.name");
	String outputFile = new Date().toString()  + userHomeDir + File.separatorChar + ".PDF";
	
	List<Product> products = new ArrayList<Product>();
	Product pr1 = new Product("IPad Air", BigDecimal.valueOf(770.2));
	Product pr2 = new Product("IPad Pro", BigDecimal.valueOf(992.8));
	
	products.add(pr1);
	products.add(pr2);
	
	JRBeanCollectionDataSource itemsJRBean = new JRBeanCollectionDataSource(products);
	Map<String, Object> parameters = new HashMap<String, Object>();
	parameters.put("ItemDataSource", itemsJRBean);
	OutputStream outputStream = null;
	try {
	    JasperReport jasperReport = JasperCompileManager.compileReport("src/main/resources/Template_Table.jrxml");
		JasperPrint jasperPrint = JasperFillManager.fillReport(jasperReport, parameters);
		try {
			 outputStream = new FileOutputStream(new File(outputFile));
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		JasperExportManager.exportReportToPdfStream(jasperPrint, outputStream);
		System.out.println("File generated...");
	} catch (JRException e) {
		// TODO Auto-generated catch block
		e.printStackTrace();
	}
	
	
}
}
